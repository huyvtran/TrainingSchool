<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/LMSsw.Master" CodeBehind="WFCalendarBoss.aspx.vb" Inherits="TrainingSchool.WFAdminCalendar" %>
<%@ Register TagPrefix="uc2" TagName="calendar" Src="~/UCCalendarBoss.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    
    
    <div class="page-content">
       

       <div class="col-xs-12">
             <uc2:calendar id="calendar1" runat="server" enabled=true />
  
   </div>
  </div> 
  </asp:Content>