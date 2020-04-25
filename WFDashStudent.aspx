<%@ Page Title="Cruscotto Studente" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSsw.Master" CodeBehind="WFDashStudent.aspx.vb" Inherits="TrainingSchool.WfDashstudent" %>



<%@ Register TagPrefix="uc1" TagName="calendar1" Src="~/UCCalendar.ascx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        $(document).ready(function () {


           // loadtree();




        });
  </script>



    <div class="row">
            <div class="col-sm-12">

                <uc1:calendar1 ID="calendar_1" runat="server" />


             
            </div>
            

        </div>


</asp:Content>




