namespace DecenaSoluciones.POS.API.Helper
{
    public static class RemoveASAP
    {
        public static readonly string RemovePlease = @"<html>
  <head>
    <style type=""text/css"">
      .mainDiv {
        position: relative;
        overflow: hidden;
        margin: 42px 0px 234px 10px;
        padding: 0px;
        width: 759px;
      }

      .leftPanel {
        float: left;
        margin: 50px 0px 0px 0px;
        padding: 0px;
        width: 590px;
        overflow: hidden;
      }

      .rightPanel {
        float: right;
        margin: 30px 0px 0px 10px;
        padding: 0px;
        width: 155px;
        overflow: hidden;
        text-align: right;
      }

      .rightPanel.Bottom {
        margin: 0 !important;
        width: 300px !important;
      }

      .leftPanel.Bottom {
        margin: 0 !important;
        width: 300px !important;
      }

      .header {
        position: initial;
        top: 0px;
        left: 35px;
        z-index: -1;
        width: 760px;
        height: 160px;
      }

      .headerImage {
        width: 760px;
        height: 160px;
      }

      .tblProducts {
        font-family: ""Times New Roman"", Times, serif;
        border-collapse: collapse;
        width: 100%;
      }

      .tblProducts th {
        border: 1px solid #ddd;
        padding: 8px;
      }

      .tblProducts td {
        border-right: 1px solid #ddd;
        border-left: 1px solid #ddd;
      }

      .tblProducts tr:nth-child(even) {
        background-color: #f2f2f2;
      }

      .tblProducts th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: left;
        background-color: #bfbfbf;
      }

      .rowTotal {
        background-color: white !important;
        font-weight: bold;
      }

      .rowTotal.firstTotal {
        border-top: 1px solid #ddd;
      }

      .rowTotal td {
        border-right: 0 !important;
        border-left: 0 !important;
        padding-top: 5px;
      }

      .cellTotal {
        border: 0 !important;
      }

      .ClientP {
        position: relative;
      }

      .ClientP:before {
        content: """";
        position: absolute;
        top: 95%; /* line position can be changed according to requirment either top:0, top:50% or bottom:0*/
        left: 0;
        width: 100%;
        height: 1px;
        background: black;
      }

      .ClientP span {
        display: inline-block;
        background: #fff;
        position: relative;
        padding-right: 5px; /*space between text and line*/
      }

      hr {
        border: 0;
        border-top: 2px solid;
      }
    </style>
  </head>
  <body>
    <div class=""mainDiv"">
      <div class=""leftPanel"">
        <p class=""ClientP"">
          <span style=""font-weight: bold; font-size: 18px"">Cliente: </span
          >{{ClientName}}
        </p>
        {{PaymentConditions}}
      </div>
      <div class=""rightPanel"">
        <p>{{CreationDate}}</p>
        <p class="""">{{SaleCode}} <span class="""">{{SaleTitle}}</span></p>
      </div>

      <table class=""tblProducts"">
        <tr>
          <th>ITEMS</th>
          <th>PRODUCTO</th>
          <th>CANT.</th>
          <th>PRECIO</th>
          <th>ITBIS</th>
          <th>TOTAL</th>
        </tr>
        {{Products}}
        <tr class=""rowTotal firstTotal"">
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td>SUBTOTAL</td>
          <td>{{SubTotal}}</td>
        </tr>
        <tr class=""rowTotal"">
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td>ITBIS</td>
          <td>{{totalTaxes}}</td>
        </tr>
        <tr class=""rowTotal"">
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td>DESCUENTOS</td>
          <td>{{Discount}}</td>
        </tr>
        <tr class=""rowTotal"">
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td class=""cellTotal""></td>
          <td>TOTAL</td>
          <td>{{GrandTotal}}</td>
        </tr>
      </table>
      <div class=""leftPanel Bottom"">
        <br />
        <br />
        <hr />
        <p style=""text-align: center"" class="""">ENTREGADO</p>
      </div>
      <div class=""rightPanel Bottom"">
        <br />
        <br />
        <hr />
        <p style=""text-align: center"" class="""">RECIBIDO CONFORME</p>
      </div>
    </div>
  </body>
</html>
";
    }
}
