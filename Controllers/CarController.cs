using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CodeMetrics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MVC_CRUD.Data;
using MVC_CRUD.Models;

namespace MVC_CRUD.Controllers
{
    public class CarController : Controller
    {
        private readonly IConfiguration _configuration;
       

        public CarController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Car
        public  IActionResult Index()
        {   
            DataTable  dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("CarViewAll", sqlConnection);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.Fill(dtbl);

            }
            return View(dtbl);
        }

        // GET: Car/AddorEdit/
        public  IActionResult AddorEdit(int? id)
        {              
                       CarViewModel carViewModel = new CarViewModel();
                       if(id > 0)
            {
                carViewModel = FetchCarByID(id);
            }
                       return View(carViewModel);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddorEdit(int id, [Bind("carID,Brand,price,model")] CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlcmd = new SqlCommand("CarAddorEdit", sqlConnection);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("carID" , carViewModel.carID);
                    sqlcmd.Parameters.AddWithValue("Brand", carViewModel.Brand);
                    sqlcmd.Parameters.AddWithValue("model", carViewModel.model);
                    sqlcmd.Parameters.AddWithValue("price", carViewModel.Price);
                    sqlcmd.ExecuteNonQuery();

                }
                    return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Car/Delete/5
        public IActionResult Delete(int? id)
        {
            CarViewModel carViewModel = FetchCarByID(id);  
            return View(carViewModel);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand("CarDeleteByID", sqlConnection);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("carID", id);
                sqlcmd.ExecuteNonQuery();

            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public CarViewModel FetchCarByID(int? id)
        {
            CarViewModel carViewModel = new CarViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("CarViewByID", sqlConnection);
                sqlda.SelectCommand.Parameters.AddWithValue("carID", id);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    DataRow dataRow = dtbl.Rows[0];
                    carViewModel.carID = Convert.ToInt32(dataRow["carID"].ToString());
                    carViewModel.Brand = dataRow["Brand"].ToString();
                    carViewModel.model = dataRow["model"].ToString();
                    carViewModel.Price = Convert.ToInt32(dataRow["Price"].ToString());
                }
                return carViewModel;
            }
         
        }
       
    }
}
