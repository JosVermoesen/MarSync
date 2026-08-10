using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class PeppolDocumentRepository
{
    private readonly string _connectionString;

    public void EnsurePeppolTableExists(string mdbPath)
    {
        string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={mdbPath};";
        using (var conn = new OleDbConnection(connStr))
        {
            conn.Open();

            // Check of tabel bestaat
            DataTable tables = conn.GetSchema("Tables");
            bool exists = false;

            foreach (DataRow row in tables.Rows)
            {
                object tableNameValue = row["TABLE_NAME"];
                if (tableNameValue != null && tableNameValue != DBNull.Value &&
                    string.Equals(tableNameValue.ToString(), "PeppolDocuments", StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                string ddl = @"
                CREATE TABLE PeppolDocuments
                (
                    DocumentId      TEXT(100)    NOT NULL,
                    FilePath        TEXT(255),
                    HashSha256      TEXT(64),
                    ReceivedOn      DATETIME,
                    CONSTRAINT PK_PeppolDocuments PRIMARY KEY (DocumentId)
                );
            ";

                using (var cmd = new OleDbCommand(ddl, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public PeppolDocumentRepository(string mdbPath)
    {
        // Voor een klassieke .mdb (Jet)
        _connectionString =
            $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={mdbPath};Persist Security Info=False;";
        // Voor .accdb zou je ACE moeten gebruiken:
        // Provider=Microsoft.ACE.OLEDB.12.0;
    }

    // ---------- HASH BEREKENEN ----------

    public string ComputeSha256(string filePath)
    {
        using (var stream = File.OpenRead(filePath))
        using (var sha = SHA256.Create())
        {
            var hashBytes = sha.ComputeHash(stream);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    // ---------- REGISTREREN BIJ ONTVANGST ----------

    public void RegisterIncomingDocument(string documentId, string filePath)
    {
        string hash = ComputeSha256(filePath);

        using (var conn = new OleDbConnection(_connectionString))
        {
            conn.Open();

            // Eerst kijken of het document al bestaat
            string checkSql = "SELECT COUNT(*) FROM PeppolDocuments WHERE DocumentId = ?";
            using (var checkCmd = new OleDbCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@p1", documentId);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                string sql;
                if (count == 0)
                {
                    // Insert
                    sql = @"INSERT INTO PeppolDocuments
                            (DocumentId, FilePath, HashSha256, ReceivedOn)
                            VALUES (?, ?, ?, ?)";
                }
                else
                {
                    // Update (bijvoorbeeld bij herontvangst)
                    sql = @"UPDATE PeppolDocuments
                            SET FilePath = ?, HashSha256 = ?, ReceivedOn = ?
                            WHERE DocumentId = ?";
                }

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    if (count == 0)
                    {
                        cmd.Parameters.AddWithValue("@p1", documentId);
                        cmd.Parameters.AddWithValue("@p2", filePath);
                        cmd.Parameters.AddWithValue("@p3", hash);
                        var dateParamInsert = cmd.Parameters.Add("@p4", OleDbType.Date);
                        dateParamInsert.Value = DateTime.Now;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@p1", filePath);
                        cmd.Parameters.AddWithValue("@p2", hash);
                        var dateParamUpdate = cmd.Parameters.Add("@p3", OleDbType.Date);
                        dateParamUpdate.Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@p4", documentId);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    // ---------- CONTROLE VÓÓR VERWERKING ----------

    public bool IsUnchanged(string documentId, string filePath)
    {
        string storedHash = null;

        using (var conn = new OleDbConnection(_connectionString))
        {
            conn.Open();
            string sql = "SELECT HashSha256 FROM PeppolDocuments WHERE DocumentId = ?";
            using (var cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@p1", documentId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    storedHash = result.ToString();
            }
        }

        if (string.IsNullOrEmpty(storedHash))
            return false; // geen registratie → liever blokkeren of apart behandelen

        string currentHash = ComputeSha256(filePath);
        return string.Equals(storedHash, currentHash, StringComparison.OrdinalIgnoreCase);
    }

    public void UpdateReceivedOnIfHashMatches(string documentId, string filePath)
    {
        string storedHash = null;

        using (var conn = new OleDbConnection(_connectionString))
        {
            conn.Open();
            string sql = "SELECT HashSha256 FROM PeppolDocuments WHERE DocumentId = ?";
            using (var cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@p1", documentId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    storedHash = result.ToString();
            }
        }

        if (string.IsNullOrEmpty(storedHash))
            return; // niets te doen, geen eerdere registratie

        string currentHash = ComputeSha256(filePath);

        if (!string.Equals(storedHash, currentHash, StringComparison.OrdinalIgnoreCase))
            return; // hash verschilt → niet updaten, eventueel later behandelen

        // Hash is identiek → enkel datum bijwerken
        using (var conn = new OleDbConnection(_connectionString))
        {
            conn.Open();
            string sql = @"UPDATE PeppolDocuments
                       SET ReceivedOn = ?
                       WHERE DocumentId = ?";

            using (var cmd = new OleDbCommand(sql, conn))
            {
                var dateParam = cmd.Parameters.Add("@p1", OleDbType.Date);
                dateParam.Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@p2", documentId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
