<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="tutor.Default" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
                        <asp:TextBox ID="compilationResult" runat="server" Height="226px" TextMode="MultiLine" Width="309px" />
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="Code" runat="server" Height="226px" TextMode="MultiLine" Width="417px" />
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
        </table>
        <br />
                            <div>
                        <asp:Label ID="Label1" runat="server" />
                    </div>
    </form>
</body>
</html>
