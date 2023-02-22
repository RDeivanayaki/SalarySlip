using System.Data.SqlClient;
using System.Data;
using SalarySlip.API.Models.Domain;
using ExcelDataReader;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SalarySlip.API.Repositories
{
    public class SalaryForMonthYearRepository : ISalaryForMonthYearRepository
    {
        SqlConnection con;
        private readonly IConfiguration _iConfiguration;
        private readonly IHostEnvironment _hostEnvironment;
        public SalaryForMonthYearRepository(IConfiguration iConfiguration, IHostEnvironment hostEnvironment)
        {
            _iConfiguration = iConfiguration;
            con = new SqlConnection(_iConfiguration.GetConnectionString("SalarySlip"));
            _hostEnvironment = hostEnvironment;
        }
        public string Add(SalaryForMonthYear salary)
        {
            var filepath = _hostEnvironment.ContentRootPath + "Upload\\" + salary.UploadedFilename;
            
            int MonthlySalaryId = 0;
            SqlDataAdapter da = new SqlDataAdapter("usp_GetMaxId", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MonthlySalaryId = Convert.ToInt32(dt.Rows[0]["MonthlySalaryId"]);
            }
            MonthlySalaryId++;
            string excelreadingmsg = ReadExFile(filepath,MonthlySalaryId,salary);
            string msg = "";
            if (excelreadingmsg == "")
                msg = "Added Successfully!";
            else
                msg = excelreadingmsg;
            return msg;
        }

        public void CreateSalarySlip(List<MonthlySalaryDetail> salarydet)
        {
            DataTable tbl = new DataTable();

            tbl.Columns.Add(new DataColumn("BranchCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("EmployeeName", typeof(string)));
            tbl.Columns.Add(new DataColumn("EmployeeNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("Salary", typeof(Int32)));
            //tbl.Columns.Add(new DataColumn("Salary", typeof(string)));
            tbl.Columns.Add(new DataColumn("MonthlySalaryId", typeof(Int32)));

            foreach (var salary in salarydet)
            {
                DataRow dr = tbl.NewRow();
                dr["BranchCode"] = salary.BranchCode;
                dr["EmployeeName"] = salary.EmployeeName;
                dr["EmployeeNo"] = salary.EmployeeNo;
                dr["Salary"] = salary.NetPay;
                dr["MonthlySalaryId"] = salary.MonthlySalaryId;

                tbl.Rows.Add(dr);
            }
            
            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            //assign Destination table name  
            objbulk.DestinationTableName = "SalarySlipDetail";

            objbulk.ColumnMappings.Add("BranchCode", "BranchCode");
            objbulk.ColumnMappings.Add("EmployeeName", "EmployeeName");
            objbulk.ColumnMappings.Add("EmployeeNo", "EmployeeNo");
            objbulk.ColumnMappings.Add("Salary", "Salary");
            objbulk.ColumnMappings.Add("MonthlySalaryId", "MonthlySalaryId");

            con.Open();
            //insert bulk Records into DataBase.  
            objbulk.WriteToServer(tbl);
            con.Close();
        }

        public string GetMonth()
        {
            string month = "";
            SqlDataAdapter da = new SqlDataAdapter("usp_GetMaxId", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                month = dt.Rows[0]["Month"].ToString();
            }
            return month;
        }
        public List<SalarySlipDetail> GeneratePdf()
        {   
            //convert datatable tbl into pdf format file
            
            List<SalarySlipDetail> ssd = new List<SalarySlipDetail>();
            SqlDataAdapter da = new SqlDataAdapter("usp_GetSalarySlipDetail", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssd.Add(new SalarySlipDetail()
                    {
                        BranchCode = dt.Rows[i]["BranchCode"].ToString(),
                        EmployeeName = dt.Rows[i]["EmployeeName"].ToString(),
                        EmployeeNo = dt.Rows[i]["EmployeeNo"].ToString(),
                        Salary = Convert.ToInt32(dt.Rows[i]["Salary"]),
                        //Salary = dt.Rows[i]["Salary"].ToString(),
                    });
                }
            }
            return ssd;
        }
        public string ReadExFile(string filelocation, int monthlysalaryId, SalaryForMonthYear salary)
        {
            List<MonthlySalaryDetail> salarydet = new List<MonthlySalaryDetail>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            
            string ErrorMsg = "";
            using (var stream = System.IO.File.OpenRead(filelocation))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int totalColumns = reader.FieldCount;
                    
                    if (totalColumns == 20)
                    {
                        reader.Read();
            
                        while (reader.Read())
                        {
                            int sno = Convert.ToInt32(reader.GetValue(0));
                            string salarytype = "Nil";
                            if (reader.GetValue(1) != null)
                            {
                                
                                if (!string.IsNullOrEmpty(reader.GetValue(1).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(1).ToString()))
                                    salarytype = reader.GetValue(1).ToString();
                            }
                            string branchcode = "Nil";
                            if (reader.GetValue(2) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(2).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(2).ToString()))
                                    branchcode = reader.GetValue(2).ToString();
                                else
                                    ErrorMsg += ",BranchCode";
                            }
                            else
                                ErrorMsg += ",BranchCode";
                            
                            string employeeno = "Nil";
                            if (reader.GetValue(3) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(3).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(3).ToString()))
                                    employeeno = reader.GetValue(3).ToString();
                                else
                                    ErrorMsg += ",EmployeeNo";
                            }
                            else
                                ErrorMsg += ",EmployeeNo";
                            
                            string employeename = "Nil";
                            if (reader.GetValue(4) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(4).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(4).ToString()))
                                    employeename = reader.GetValue(4).ToString();
                                else
                                    ErrorMsg += ",EmployeeName";
                            }
                            else
                                ErrorMsg += ",EmployeeName";
                            
                            string department = "Nil";
                            if (reader.GetValue(5) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(5).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(5).ToString()))
                                    department = reader.GetValue(5).ToString();
                            }
                            
                            string designation = "Nil";
                            if (reader.GetValue(6) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(6).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(6).ToString()))
                                    designation = reader.GetValue(6).ToString();
                            }

                            string joiningdate = "Nil";
                            if (reader.GetValue(7) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(7).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(7).ToString()))
                                    joiningdate = reader.GetValue(7).ToString();
                            }
                            
                            double monthlygross = 0.0;
                            if (reader.GetValue(8) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(8).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(8).ToString()))
                                    monthlygross = Convert.ToDouble(reader.GetValue(8));
                            }

                            if (monthlygross <= 0.0)
                                ErrorMsg += ",Monthly Gross";

                            double workDays = 0.0;
                            if (reader.GetValue(9) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(9).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(9).ToString()))
                                    workDays = Convert.ToDouble(reader.GetValue(9));
                            }
                            if (workDays < 0.0)
                                ErrorMsg += ",Workdays";
                            
                            double extraDays = 0.0;
                            if (reader.GetValue(10) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(10).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(10).ToString()))
                                    extraDays = Convert.ToDouble(reader.GetValue(10));
                            }
                            if (extraDays < 0.0)
                                ErrorMsg += ",Extradays";
                            
                            double totalDays = workDays + extraDays;
                            if (totalDays < 0.0 && totalDays > 31)
                                ErrorMsg += ",Totaldays";
                            
                            double gross = totalDays * monthlygross / 30;
                            
                            if (gross < 0.0)
                                ErrorMsg += ",Gross";

                            double pf = 0.0;
                            if (reader.GetValue(13) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(13).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(13).ToString()))
                                    pf = Convert.ToDouble(reader.GetValue(13));
                            }
                            if (pf < 0.0)
                                ErrorMsg += ",PF";
                            
                            double esi = 0.0;
                            if (reader.GetValue(14) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(14).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(14).ToString()))
                                    esi = Convert.ToDouble(reader.GetValue(14));
                            }
                            if (esi < 0.0)
                                ErrorMsg += ",ESI";
                            
                            double loan = 0.0;
                            if (reader.GetValue(15) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(15).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(15).ToString()))
                                    loan = Convert.ToDouble(reader.GetValue(15));
                            }
                            if (loan < 0.0)
                                ErrorMsg += ",Loan";
                            
                            double salaryAdvance = 0.0;
                            if (reader.GetValue(16) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(16).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(16).ToString()))
                                    salaryAdvance = Convert.ToDouble(reader.GetValue(16));
                            }
                            if (salaryAdvance < 0.0)
                                ErrorMsg += ",SalaryAdvance";
                            
                            double shortage = 0.0;
                            if (reader.GetValue(17) != null)
                            {
                                if (!string.IsNullOrEmpty(reader.GetValue(17).ToString()) && !string.IsNullOrWhiteSpace(reader.GetValue(17).ToString()))
                                    shortage = Convert.ToDouble(reader.GetValue(17));
                            }
                            if (shortage < 0.0)
                                ErrorMsg += ",Shortage";

                            double totalDeductions = pf + esi + loan + salaryAdvance + shortage;
                            if (totalDeductions < 0.0)
                                ErrorMsg += ",TotalDeductions";
                            
                            double netPay = gross - totalDeductions;
                            if (netPay < 0.0)
                                ErrorMsg += ",NetPay";
                            
                            if (ErrorMsg != "")
                            {
                                ErrorMsg = ErrorMsg.Substring(1);
                                ErrorMsg += " Values are invalid!";
                                break;
                            }

                            salarydet.Add(new MonthlySalaryDetail()
                            {
                                SINo = sno,
                                SalaryType = salarytype,
                                BranchCode = branchcode,
                                EmployeeNo = employeeno,
                                EmployeeName = employeename,
                                Department = department,
                                Designation = designation,
                                DateofJoining = joiningdate,
                                MonthlyGross = monthlygross,
                                Workdays = workDays,
                                ExtraDays = extraDays,
                                TotalDays = totalDays,
                                Gross = (int)Math.Round(gross),
                                PF = pf,
                                ESI = esi,
                                Loan = loan,
                                SalaryAdvance = salaryAdvance,
                                Shortage = shortage,
                                TotalDeductions = totalDeductions,
                                NetPay = (int)Math.Round(netPay,MidpointRounding.AwayFromZero),//Math.Round(netPay, MidpointRounding.AwayFromZero).ToString(), //netPay.ToString(),//(int)Math.Round(netPay),
                                MonthlySalaryId = monthlysalaryId,
                            }
                            ); ;
                        }
                    }
                    else
                        ErrorMsg = "Some fields are missing in the excel file!";
                }
            }
            if (ErrorMsg == "")
            {
                if (salarydet.Count > 0) //only column headers available without data case
                {
                    if (DataInsertionForSalaryForMonthYear(monthlysalaryId, salary) == "Success")
                    {
                        ExcelFileDataInsertion(salarydet);
                        CreateSalarySlip(salarydet);

                    }
                }
                else
                    ErrorMsg = "No data found in the excel file!";
            }
            else
                salarydet.Clear();
                
            return ErrorMsg;
        }

    public string DataInsertionForSalaryForMonthYear(int monthlysalaryid,SalaryForMonthYear salary)
    {
            SqlCommand cmd = new SqlCommand("usp_AddSalary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@MonthlySalaryId", monthlysalaryid);
            cmd.Parameters.AddWithValue("@Month", salary.Month);
            cmd.Parameters.AddWithValue("@Year", salary.Year);
            cmd.Parameters.AddWithValue("@UploadedFilename", salary.UploadedFilename);
            
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            string errormsg = "";
            if (i > 0)
                errormsg = "Success";
            else
                errormsg = "Error";
            return errormsg;
        }
    public void ExcelFileDataInsertion(List<MonthlySalaryDetail> salarydet)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("SINo", typeof(Int32)));
            tbl.Columns.Add(new DataColumn("SalaryType", typeof(string)));
            tbl.Columns.Add(new DataColumn("BranchCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("EmployeeNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("EmployeeName", typeof(string)));
            tbl.Columns.Add(new DataColumn("Department", typeof(string)));
            tbl.Columns.Add(new DataColumn("Designation", typeof(string)));
            tbl.Columns.Add(new DataColumn("DateofJoining", typeof(string)));
            tbl.Columns.Add(new DataColumn("MonthlyGross", typeof(float)));
            tbl.Columns.Add(new DataColumn("Workdays", typeof(float)));
            tbl.Columns.Add(new DataColumn("ExtraDays", typeof(float)));
            tbl.Columns.Add(new DataColumn("TotalDays", typeof(float)));
            tbl.Columns.Add(new DataColumn("Gross", typeof(Int32)));
            tbl.Columns.Add(new DataColumn("PF", typeof(float)));
            tbl.Columns.Add(new DataColumn("ESI", typeof(float)));
            tbl.Columns.Add(new DataColumn("Loan", typeof(float)));
            tbl.Columns.Add(new DataColumn("SalaryAdvance", typeof(float)));
            tbl.Columns.Add(new DataColumn("Shortage", typeof(float)));
            tbl.Columns.Add(new DataColumn("TotalDeductions", typeof(float)));
            tbl.Columns.Add(new DataColumn("NetPay", typeof(Int32)));
            //tbl.Columns.Add(new DataColumn("NetPay", typeof(string)));
            tbl.Columns.Add(new DataColumn("MonthlySalaryId", typeof(Int32)));
            
            foreach (var salary in salarydet)
            {
                DataRow dr = tbl.NewRow();
                dr["SINo"] = salary.SINo;
                dr["SalaryType"] = salary.SalaryType;
                dr["BranchCode"] = salary.BranchCode;
                dr["EmployeeNo"] = salary.EmployeeNo;
                dr["EmployeeName"] = salary.EmployeeName;
                dr["Department"] = salary.Department;
                dr["Designation"] = salary.Designation;
                dr["DateofJoining"] = salary.DateofJoining;
                dr["MonthlyGross"] = salary.MonthlyGross;
                dr["Workdays"] = salary.Workdays;
                dr["ExtraDays"] = salary.ExtraDays;
                dr["TotalDays"] = salary.TotalDays;
                dr["Gross"] = salary.Gross;
                dr["PF"] = salary.PF;
                dr["ESI"] = salary.ESI;
                dr["Loan"] = salary.Loan;
                dr["SalaryAdvance"] = salary.SalaryAdvance;
                dr["Shortage"] = salary.Shortage;
                dr["TotalDeductions"] = salary.TotalDeductions;
                dr["NetPay"] = salary.NetPay;
                dr["MonthlySalaryId"] = salary.MonthlySalaryId;

                tbl.Rows.Add(dr);
            }
            
            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            //assign Destination table name  
            objbulk.DestinationTableName = "MonthlySalaryDetail";

            objbulk.ColumnMappings.Add("SINo", "SINo");
            objbulk.ColumnMappings.Add("SalaryType", "SalaryType");
            objbulk.ColumnMappings.Add("BranchCode", "BranchCode");
            objbulk.ColumnMappings.Add("EmployeeNo", "EmployeeNo");
            objbulk.ColumnMappings.Add("EmployeeName", "EmployeeName");
            objbulk.ColumnMappings.Add("Department", "Department");
            objbulk.ColumnMappings.Add("Designation", "Designation");
            objbulk.ColumnMappings.Add("DateofJoining", "DateofJoining");
            objbulk.ColumnMappings.Add("MonthlyGross", "MonthlyGross");
            objbulk.ColumnMappings.Add("Workdays", "Workdays");
            objbulk.ColumnMappings.Add("ExtraDays", "ExtraDays");
            objbulk.ColumnMappings.Add("TotalDays", "TotalDays");
            objbulk.ColumnMappings.Add("Gross", "Gross");
            objbulk.ColumnMappings.Add("PF", "PF");
            objbulk.ColumnMappings.Add("ESI", "ESI");
            objbulk.ColumnMappings.Add("Loan", "Loan");
            objbulk.ColumnMappings.Add("SalaryAdvance", "SalaryAdvance");
            objbulk.ColumnMappings.Add("Shortage", "Shortage");
            objbulk.ColumnMappings.Add("TotalDeductions", "TotalDeductions");
            objbulk.ColumnMappings.Add("NetPay", "NetPay");
            objbulk.ColumnMappings.Add("MonthlySalaryId", "MonthlySalaryId");
            
            con.Open();
            //insert bulk Records into DataBase.  
            objbulk.WriteToServer(tbl);
            con.Close();
        }
    }
}


