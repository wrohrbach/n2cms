﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemXmlImport.ascx.cs" Inherits="N2.Management.Content.Export.ItemXmlImport" %>
<%@ Register Src="../AffectedItems.ascx" TagName="AffectedItems" TagPrefix="uc1" %>

<asp:CustomValidator id="cvImport" runat="server" CssClass="validator" meta:resourceKey="cvImport" Display="Dynamic"/>

<div>
	<asp:CheckBox ID="chkSkipRoot" runat="server" Text="Skip imported root item" ToolTip="Checking this options cause the first level item not to be imported, and it's children to be added to the selected item's children" meta:resourceKey="chkSkipRoot" />
</div>
<asp:Button ID="btnImportUploaded" runat="server" Text="Import" OnClick="btnImportUploaded_Click"  meta:resourceKey="btnImportUploaded"/>
<n2:h4 runat="server" Text="Imported Items" meta:resourceKey="importedItems" />
<uc1:AffectedItems id="importedItems" runat="server" />
<n2:h4 runat="server" Text="Attachments" meta:resourceKey="attachments" />
<asp:Repeater ID="rptAttachments" runat="server">
	<ItemTemplate>
		<div class="file"><asp:Image ID="Image1" runat="server" ImageUrl="../../Resources/icons/page_white.png" alt="file" /><%# Eval("Url") %> <span class="warning"><%# CheckExists((string)Eval("Url")) %></span></div>
	</ItemTemplate>
</asp:Repeater>