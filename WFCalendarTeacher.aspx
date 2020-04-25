<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/LMSsw.Master" CodeBehind="WFCalendarTeacher.aspx.vb" Inherits="TrainingSchool.WFHome" %>
<%@ Register TagPrefix="uc2" TagName="calendar" Src="~/UCCalendarTeacher.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    
    
    <div class="page-content">
       

       <div class="col-xs-12">
             <uc2:calendar id="calendar1" Visible="false" setattendance="1" runat="server" enabled=true />
         
  
   </div>
  </div> 
  </asp:Content>