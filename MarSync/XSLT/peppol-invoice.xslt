<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
    xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
    xmlns:inv="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"
    xmlns:crn="urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2"
    exclude-result-prefixes="cac cbc inv crn">

  <xsl:output method="html" encoding="UTF-8" indent="yes"/>

  <!-- Determine if document is invoice or credit note -->
  <xsl:variable name="rootName" select="local-name(/*)"/>

  <xsl:variable name="docTypeCode">
    <xsl:choose>
      <xsl:when test="//cbc:InvoiceTypeCode">
        <xsl:value-of select="//cbc:InvoiceTypeCode"/>
      </xsl:when>
      <xsl:when test="//cbc:CreditNoteTypeCode">
        <xsl:value-of select="//cbc:CreditNoteTypeCode"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="docTypeText">
    <xsl:choose>
      <xsl:when test="$rootName = 'CreditNote'">Creditnota</xsl:when>
      <xsl:when test="$docTypeCode = '381'">Creditnota</xsl:when>
      <xsl:otherwise>Factuur</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="isCredit">
    <xsl:choose>
      <xsl:when test="$rootName = 'CreditNote'">true</xsl:when>
      <xsl:when test="$docTypeCode = '381'">true</xsl:when>
      <xsl:otherwise>false</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:template match="/">
    <html>
      <head>
        <meta charset="UTF-8"/>
        <title>
          <xsl:value-of select="$docTypeText"/>
          <xsl:text> - </xsl:text>
          <xsl:value-of select="//cbc:ID"/>
        </title>

        <style type="text/css">
          body {
          font-family: 'Segoe UI', Arial, sans-serif;
          font-size: 13px;
          color: #1a1a1a;
          margin: 0;
          background-color: #fafafa;
          }

          :root {
          --vsoft-blue: #0A3A67;
          --vsoft-orange: #F28C28;
          --light-gray: #e6e6e6;
          }

          .header-bar {
          background-color: var(--vsoft-blue);
          color: white;
          padding: 15px 20px;
          display: flex;
          justify-content: space-between;
          align-items: center;
          }

          .header-title {
          font-size: 22px;
          font-weight: 600;
          }

          .logo {
          font-size: 18px;
          font-weight: bold;
          color: var(--vsoft-orange);
          }

          .banner {
          background-color: var(--vsoft-orange);
          color: white;
          padding: 10px 20px;
          font-weight: bold;
          text-align: center;
          }

          .section {
          margin: 20px;
          background: white;
          padding: 15px;
          border-radius: 6px;
          border: 1px solid var(--light-gray);
          }

          h2 {
          color: var(--vsoft-blue);
          border-bottom: 2px solid var(--vsoft-blue);
          padding-bottom: 4px;
          margin-bottom: 10px;
          font-size: 16px;
          }

          table {
          border-collapse: collapse;
          width: 100%;
          margin-top: 10px;
          }

          th {
          background-color: var(--vsoft-blue);
          color: white;
          padding: 6px;
          text-align: left;
          font-weight: 500;
          }

          td {
          border: 1px solid #ddd;
          padding: 6px;
          }

          .right { text-align: right; }

          .summary-table td {
          border: none;
          padding: 3px 4px;
          }

          .summary-table .label { text-align: right; width: 70%; }
          .summary-table .value { text-align: right; width: 30%; font-weight: bold; }
        </style>
      </head>

      <body>

        <div class="header-bar">
          <div class="header-title">
            <xsl:text>Peppol </xsl:text>
            <xsl:value-of select="$docTypeText"/>
          </div>
          <div class="logo">Vsoft 1985</div>
        </div>

        <div class="banner">
          Dit is de officiële XML‑weergave
          <xsl:text> van de </xsl:text>
          <xsl:value-of select="$docTypeText"/>
        </div>

        <div class="section">
          <h2>
            <xsl:value-of select="$docTypeText"/>
            <xsl:text>gegevens</xsl:text>
          </h2>
          <b>
            <xsl:value-of select="$docTypeText"/>
            <xsl:text>nummer:</xsl:text>
          </b>
          <xsl:text> </xsl:text>
          <xsl:value-of select="//cbc:ID"/>
          <br/>
          <b>
            <xsl:value-of select="$docTypeText"/>
            <xsl:text>datum:</xsl:text>
          </b>
          <xsl:text> </xsl:text>
          <xsl:value-of select="//cbc:IssueDate"/>
          <br/>
          <b>Vervaldatum:</b>
          <xsl:text> </xsl:text>
          <xsl:value-of select="//cbc:DueDate"/>

          <xsl:if test="$isCredit = 'true' and //cac:BillingReference/cac:InvoiceDocumentReference/cbc:ID">
            <br/>
            <b>Origineel factuurnummer:</b>
            <xsl:text> </xsl:text>
            <xsl:value-of select="//cac:BillingReference/cac:InvoiceDocumentReference/cbc:ID"/>
          </xsl:if>
        </div>

        <div class="section">
          <h2>Leverancier</h2>
          <xsl:apply-templates select="//cac:AccountingSupplierParty/cac:Party"/>
        </div>

        <div class="section">
          <h2>Klant</h2>
          <xsl:apply-templates select="//cac:AccountingCustomerParty/cac:Party"/>
        </div>

        <div class="section">
          <h2>Betalingsgegevens</h2>
          <xsl:apply-templates select="//cac:PaymentMeans"/>
        </div>

        <div class="section">
          <h2>
            <xsl:choose>
              <xsl:when test="$isCredit = 'true'">Creditnota‑lijnen</xsl:when>
              <xsl:otherwise>Factuurlijnen</xsl:otherwise>
            </xsl:choose>
          </h2>
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Omschrijving</th>
                <th class="right">Aantal</th>
                <th>Eenheid</th>
                <th class="right">Prijs</th>
                <th class="right">BTW %</th>
                <th class="right">
                  <xsl:choose>
                    <xsl:when test="$isCredit = 'true'">Lijnbedrag (credit)</xsl:when>
                    <xsl:otherwise>Lijnbedrag</xsl:otherwise>
                  </xsl:choose>
                </th>
              </tr>
            </thead>
            <tbody>
              <xsl:choose>
                <xsl:when test="$isCredit = 'true'">
                  <xsl:apply-templates select="//cac:CreditNoteLine"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="//cac:InvoiceLine"/>
                </xsl:otherwise>
              </xsl:choose>
            </tbody>
          </table>
        </div>

        <div class="section">
          <h2>Samenvatting</h2>
          <table class="summary-table">
            <tr>
              <td class="label">Subtotaal lijnen excl. BTW</td>
              <td class="value">
                <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:LineExtensionAmount"/>
                <xsl:text> </xsl:text>
                <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
              </td>
            </tr>

            <!-- Show allowances (discounts) if present -->
            <xsl:if test="//cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount">
              <tr>
                <td class="label">Kortingen</td>
                <td class="value">
                  <xsl:text>-</xsl:text>
                  <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount"/>
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
                </td>
              </tr>
            </xsl:if>

            <!-- Show charges (additional fees like delivery) if present -->
            <xsl:if test="//cac:LegalMonetaryTotal/cbc:ChargeTotalAmount">
              <tr>
                <td class="label">Toeslagen (levering, verzending, ...)</td>
                <td class="value">
                  <xsl:text>+</xsl:text>
                  <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:ChargeTotalAmount"/>
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
                </td>
              </tr>
            </xsl:if>

            <!-- Tax Exclusive Amount (after allowances/charges, before VAT) -->
            <xsl:if test="//cac:LegalMonetaryTotal/cbc:TaxExclusiveAmount">
              <tr>
                <td class="label">Totaal excl. BTW</td>
                <td class="value">
                  <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:TaxExclusiveAmount"/>
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
                </td>
              </tr>
            </xsl:if>

            <tr>
              <td class="label">BTW</td>
              <td class="value">
                <xsl:value-of select="//cac:TaxTotal/cbc:TaxAmount"/>
                <xsl:text> </xsl:text>
                <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
              </td>
            </tr>

            <!-- Tax Inclusive Amount -->
            <xsl:if test="//cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount">
              <tr>
                <td class="label">
                  <b>Totaal incl. BTW</b>
                </td>
                <td class="value">
                  <b>
                    <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount"/>
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
                  </b>
                </td>
              </tr>
            </xsl:if>

            <!-- Prepaid Amount if present -->
            <xsl:if test="//cac:LegalMonetaryTotal/cbc:PrepaidAmount">
              <tr>
                <td class="label">Reeds betaald</td>
                <td class="value">
                  <xsl:text>-</xsl:text>
                  <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:PrepaidAmount"/>
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
                </td>
              </tr>
            </xsl:if>

            <tr>
              <td class="label">
                <b>
                  <xsl:choose>
                    <xsl:when test="$isCredit = 'true'">Te crediteren</xsl:when>
                    <xsl:otherwise>Te betalen</xsl:otherwise>
                  </xsl:choose>
                </b>
              </td>
              <td class="value">
                <b>
                  <xsl:value-of select="//cac:LegalMonetaryTotal/cbc:PayableAmount"/>
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="//cbc:DocumentCurrencyCode"/>
                </b>
              </td>
            </tr>
          </table>
        </div>

      </body>
    </html>
  </xsl:template>

  <!-- Leverancier / Klant -->
  <xsl:template match="cac:Party">
    <div>
      <b>
        <!-- Prefer RegistrationName, fall back to PartyName/Name -->
        <xsl:choose>
          <xsl:when test="cac:PartyLegalEntity/cbc:RegistrationName">
            <xsl:value-of select="cac:PartyLegalEntity/cbc:RegistrationName"/>
          </xsl:when>
          <xsl:when test="cac:PartyName/cbc:Name">
            <xsl:value-of select="cac:PartyName/cbc:Name"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>(Naam onbekend)</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </b>
      <br/>
      <xsl:value-of select="cac:PostalAddress/cbc:StreetName"/>
      <xsl:text> </xsl:text>
      <xsl:value-of select="cac:PostalAddress/cbc:BuildingNumber"/>
      <br/>
      <xsl:value-of select="cac:PostalAddress/cbc:PostalZone"/>
      <xsl:text> </xsl:text>
      <xsl:value-of select="cac:PostalAddress/cbc:CityName"/>
      <br/>
      <xsl:value-of select="cac:PostalAddress/cac:Country/cbc:IdentificationCode"/>
      <br/>

      <xsl:if test="cac:PartyTaxScheme/cbc:CompanyID">
        BTW: <xsl:value-of select="cac:PartyTaxScheme/cbc:CompanyID"/><br/>
      </xsl:if>

      <xsl:if test="cac:Contact/cbc:ElectronicMail">
        E-mail: <xsl:value-of select="cac:Contact/cbc:ElectronicMail"/><br/>
      </xsl:if>
    </div>
  </xsl:template>

  <!-- Betaling -->
  <xsl:template match="cac:PaymentMeans">
    <div>
      <xsl:if test="cbc:PaymentMeansCode">
        <b>Wijze:</b>
        <xsl:text> </xsl:text>
        <xsl:value-of select="cbc:PaymentMeansCode"/>
        <br/>
      </xsl:if>
      <xsl:if test="cac:PayeeFinancialAccount/cbc:ID">
        <b>IBAN:</b>
        <xsl:text> </xsl:text>
        <xsl:value-of select="cac:PayeeFinancialAccount/cbc:ID"/>
        <br/>
      </xsl:if>
      <xsl:if test="//cbc:PaymentID">
        <b>Mededeling:</b>
        <xsl:text> </xsl:text>
        <xsl:value-of select="//cbc:PaymentID"/>
        <br/>
      </xsl:if>
    </div>
  </xsl:template>

  <!-- Lijnen: handle both InvoiceLine and CreditNoteLine -->
  <xsl:template match="cac:InvoiceLine | cac:CreditNoteLine">
    <tr>
      <td>
        <xsl:value-of select="cbc:ID"/>
      </td>
      <td>
        <xsl:value-of select="cac:Item/cbc:Name"/>
      </td>
      <td class="right">
        <xsl:choose>
          <xsl:when test="cbc:InvoicedQuantity">
            <xsl:value-of select="cbc:InvoicedQuantity"/>
          </xsl:when>
          <xsl:when test="cbc:CreditedQuantity">
            <xsl:value-of select="cbc:CreditedQuantity"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="cbc:InvoicedQuantity/@unitCode">
            <xsl:value-of select="cbc:InvoicedQuantity/@unitCode"/>
          </xsl:when>
          <xsl:when test="cbc:CreditedQuantity/@unitCode">
            <xsl:value-of select="cbc:CreditedQuantity/@unitCode"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td class="right">
        <xsl:value-of select="cac:Price/cbc:PriceAmount"/>
      </td>
      <td class="right">
        <xsl:value-of select="cac:Item/cac:ClassifiedTaxCategory/cbc:Percent"/>
      </td>
      <td class="right">
        <xsl:value-of select="cbc:LineExtensionAmount"/>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>