<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Diplom.Default" CodeBehind="App_Code/Checkers.cs" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <style>
            label {
                margin: auto; /* Выравниваем таблицу по центру окна  */
            }

            table {
                width: 600px; /* Ширина таблицы */
                border: 1px solid green; /* Рамка вокруг таблицы */
                margin: auto; /* Выравниваем таблицу по центру окна  */
            }
        </style>
        <table id="table1">
            <tr>
                <td>Задача
                </td>
                <td>Исходный код
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:TextBox ID="compilationResult" runat="server" Height="400px" TextMode="MultiLine" Width="700px" ReadOnly="true" />
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="Code" runat="server" Height="400px" TextMode="MultiLine" Width="500px" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:Button ID="cmdSubmit" runat="server" Text="Отправить" OnClick="cmdSubmit_Click" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:Label ID="Label1" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <br />

    </form>
</body>
</html>
