<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProfileManagement.aspx.cs" Inherits="ProfileManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            width: 277px;
        }
        .style2
        {
            width: 274px;
        }
        .style4
        {
            width: 850px;
        }
        .style5
        {
            width: 86px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table width="100%">
            <tr>
                <td class="style1"><asp:Label ID="lblUserName" runat="server" Text="Label">UserName : </asp:Label></td>
                <td><asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="style1"><asp:Label ID="lblProfileList" runat="server" Text="Label">ProfileTypeList : </asp:Label></td>
                <td><asp:DropDownList ID="ddlProfileList" runat="server"></asp:DropDownList></td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="style2"><asp:Label ID="lblPropertyName" runat="server" Text="PropertyName : "></asp:Label></td>
                            <td><asp:TextBox ID="txtPropertyName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="style2"><asp:Label ID="lblPropertyValue" runat="server" Text="PropertyValue : "></asp:Label></td>
                            <td><asp:TextBox ID="txtPropertyValue" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>  
                <td class="style5"><asp:Button ID="btnAdd" runat="server" Text=">>>" onclick="btnAdd_Click" /><br /><br /><asp:Button ID="btnRemove" runat="server" Text="<<<" onclick="btnRemove_Click" /></td>
                <td class="style4"><asp:ListBox ID="lstProperties" runat="server"></asp:ListBox></td>
            </tr>
         </table>
         <table width="100%">
            <tr style="height:10px">
                <td></td>
                <td><asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="red" Font-Bold="true"></asp:Label></td>
                <td></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSetProfile" runat="server" Text="Set Profile" 
                        onclick="btnSetProfile_Click" /></td>
                <td><asp:Button ID="btnGetProfile" runat="server" Text="Get Profile" 
                        onclick="btnGetProfile_Click" /></td>
                <td><asp:Button ID="btnRemoveProfile" runat="server" Text="Remove Profile" 
                        onclick="btnRemoveProfile_Click" /></td>
            </tr>
            <tr>
                <td></td>
                <td valign="top"><asp:RadioButtonList ID="rdoRetriveOptions" runat="server">
                    <asp:ListItem Value="1" Selected="True">Get all types of profile for given user</asp:ListItem>
                    <asp:ListItem Value="2">Get only mentioned properties from all types of profile 
                    for given user</asp:ListItem>
                    <asp:ListItem Value="3">Get all users’ profile for given profile type</asp:ListItem>
                    <asp:ListItem Value="4">Get only mentioned properties for all users for given 
                    profile type</asp:ListItem>
                    <asp:ListItem Value="5">Get profile for given user and profile type</asp:ListItem>
                    <asp:ListItem Value="6">Get only mentioned properties for given user and profile 
                    type</asp:ListItem>
                    <asp:ListItem Value="7">Get all profile</asp:ListItem>
                    <asp:ListItem Value="8">Get only mentioned properties from all profile</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td valign="top"><asp:RadioButtonList ID="rdoRemoveOptions" runat="server">
                    <asp:ListItem Value="1"  Selected="True">Completely remove all types of profile of given user</asp:ListItem>
                    <asp:ListItem Value="2">Just remove all the properties and values for given user and group</asp:ListItem>
                    <asp:ListItem Value="3">Remove only mentioned properties for given user and group</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <asp:Label ID="lblReportForRetrivedProfile" runat="server" 
            Text="Retrived Profile Description" Font-Bold="True" Font-Size="X-Large" 
            Font-Underline="True" ></asp:Label>
            
         <table width="100%">
            <tr>
                <td><asp:Label ID="lblReportInfo" runat="server" Text=""></asp:Label></td>
            </tr>
         </table>   
     </div>
    </form>
</body>
</html>
