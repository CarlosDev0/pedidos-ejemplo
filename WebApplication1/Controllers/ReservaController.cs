using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ReservaController : Controller
    {
        private DefaultConnection db = new DefaultConnection();
        // GET: Reserva
        public ActionResult Index()
        {
            List<TipoAlojamiento> lista = db.TipoAlojamiento.ToList();
            ViewBag.TipoAlojamiento = ToSelectList(lista, "Id", "Descripcion");

            List<Municipio> listaMun = db.Municipio.ToList();
            ViewBag.Municipio = ToSelectListMuni(listaMun, "Id", "Descripcion");

            ViewBag.lista = db.Municipio.ToList();

            return View();
        }

        public SelectList ToSelectList(List<TipoAlojamiento> tabla, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "--Seleccione--", Value = "0" });

            foreach (TipoAlojamiento row in tabla)
            {
                list.Add(new SelectListItem()
                {
                    Text = row.Descripcion,
                    Value = row.Id.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public SelectList ToSelectListMuni(List<Municipio> tabla, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "--Seleccione--", Value = "0" });

            foreach (Municipio row in tabla)
            {
                list.Add(new SelectListItem()
                {
                    Text = row.Descripcion,
                    Value = row.Id.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
        [HttpPost]
        public ActionResult Llenar(string fechaini, string fechaFin, string tipo, string personas, string municipio)
        {
            if (tipo == "")
                tipo = "0";
            if (personas == "")
                personas = "0";
            if (municipio == "")
                municipio = "0";
            List<ReservaViewModel> lst = new List<ReservaViewModel>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Habitaciones_Disponibles";
            cmd.Parameters.Add("@FechaInicio", SqlDbType.VarChar);
            cmd.Parameters.Add("@FechaFIn", SqlDbType.VarChar);
            cmd.Parameters.Add("@TipoAlojamiento", SqlDbType.Int);
            cmd.Parameters.Add("@CantPersona", SqlDbType.Int);
            cmd.Parameters.Add("@Municipio", SqlDbType.Int);
            cmd.Parameters["@FechaInicio"].Value = fechaini;
            cmd.Parameters["@FechaFIn"].Value = fechaFin;
            cmd.Parameters["@TipoAlojamiento"].Value = tipo;
            cmd.Parameters["@CantPersona"].Value = personas;
            cmd.Parameters["@Municipio"].Value = municipio;
            var result = cmd.ExecuteReader();

            while (result.Read())
            {
                ReservaViewModel datos = new ReservaViewModel();
                datos.Id = Int16.Parse(result[0].ToString());
                datos.Lugar = result[1].ToString();
                datos.Alojamiento = result[2].ToString();
                datos.TipoAlojamiento = result[3].ToString();
                datos.CapacidadMaxima = result[4].ToString();
                datos.Valor = result[5].ToString();

                lst.Add(datos);
            }
            result.Close();
            conn.Close();
            return Json(lst, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult Create(string id)
        {
            var parametros = id.Split(',');
            ViewBag.id = parametros[0];
            ViewBag.fechaInicio = parametros[1];
            ViewBag.fechaFin = parametros[2];
            ReservaViewModel lst = new ReservaViewModel();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Cargar_Reserva";
            cmd.Parameters.Add("@TarifaId", SqlDbType.Int);
            cmd.Parameters["@TarifaId"].Value = parametros[0];

            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                ReservaViewModel datos = new ReservaViewModel();
                datos.Id = Int16.Parse(result[0].ToString());
                datos.Lugar = result[1].ToString();
                datos.Alojamiento = result[2].ToString();
                datos.TipoAlojamiento = result[3].ToString();
                datos.CapacidadMaxima = result[4].ToString();
                datos.Valor = result[5].ToString();

                lst = datos;
            }
            result.Close();
            conn.Close();
            return View(lst);
        }

        [HttpPost]
        public ActionResult CrearReserva(string id, string fechaInicio, string fechaFin)
        {
            ReservaViewModel lst = new ReservaViewModel();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Crear_Reserva";
            cmd.Parameters.Add("@Usuarioid", SqlDbType.VarChar);
            cmd.Parameters.Add("@Tarifa", SqlDbType.Int);
            cmd.Parameters.Add("@fechaInicio", SqlDbType.VarChar);
            cmd.Parameters.Add("@FechaFIn", SqlDbType.VarChar);
            cmd.Parameters["@Usuarioid"].Value = User.Identity.GetUserName();
            cmd.Parameters["@Tarifa"].Value = id;
            cmd.Parameters["@fechaInicio"].Value = fechaInicio;
            cmd.Parameters["@FechaFIn"].Value = fechaFin;
            var result = cmd.ExecuteReader();
            Int32 success = 0;

            if (result.HasRows)
            {
                while (result.Read())
                {
                    success = result.GetInt32(0);
                }
            }
            conn.Close();
            return RedirectToAction("ListadoReservas","Reservas");

        }

        public ActionResult Details(string id)
        {

            EncabezadoDetalleViewModel datos = new EncabezadoDetalleViewModel();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Detalle_Alojamiento";
            cmd.Parameters.Add("@TarifaId", SqlDbType.Int);
            cmd.Parameters["@TarifaId"].Value = id;

            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                datos.Descripcion = result[0].ToString();
                datos.Lugar = result[1].ToString();
                datos.TipoAlojamiento = result[2].ToString();
                datos.Valor = result[3].ToString();
            }
            result.Close();
            conn.Close();

            @ViewBag.Alojamiento = detalle(id);
            var tabla = detalleHabitacion(id);
            @ViewBag.numeroHabi = tabla.FirstOrDefault().numHabitacion;
            @ViewBag.Habitaciones = tabla;
            return View(datos);
        }
        
        public List<DetalleAlojamientoViewModel> detalle(string id)
        {

            List<DetalleAlojamientoViewModel> list = new List<DetalleAlojamientoViewModel>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Detalle_Alojamiento_dtalle";
            cmd.Parameters.Add("@TarifaId", SqlDbType.Int);
            cmd.Parameters["@TarifaId"].Value = id;

            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                DetalleAlojamientoViewModel datos = new DetalleAlojamientoViewModel();
                datos.TipoItemId = result[0].ToString();
                datos.detalle = result[1].ToString();
                datos.Cantidad = result[2].ToString();
                list.Add(datos);
            }
            result.Close();
            conn.Close();

            return list;

        }
        public List<DetalleHabitacionViewModel> detalleHabitacion(string id)
        {

            List<DetalleHabitacionViewModel> list = new List<DetalleHabitacionViewModel>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Detalle_Alojamiento_habitacion";
            cmd.Parameters.Add("@TarifaId", SqlDbType.Int);
            cmd.Parameters["@TarifaId"].Value = id;

            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                DetalleHabitacionViewModel datos = new DetalleHabitacionViewModel();
                datos.id = result[0].ToString();
                datos.habitacion = result[1].ToString();
                datos.TipoItemId = result[2].ToString();
                datos.detalle = result[3].ToString();
                datos.Cantidad = result[4].ToString();
                datos.numHabitacion = result[5].ToString();
                list.Add(datos);
            }
            result.Close();
            conn.Close();

            return list;

        }
        public ActionResult ListadoReservas()
        {
            List<ListaReservaViewModel> lst = new List<ListaReservaViewModel>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Reservas_Registradas";
            var result = cmd.ExecuteReader();

            while (result.Read())
            {
                ListaReservaViewModel datos = new ListaReservaViewModel();
                datos.UsuarioId = result[0].ToString();
                datos.FechaInicial = result[1].ToString();
                datos.FechaFinal = result[2].ToString();
                datos.totalDiasReservados = result[3].ToString();
                datos.Alojamiento = result[4].ToString();
                datos.Sede = result[5].ToString();
                datos.Municipio = result[6].ToString();
                datos.Departamento = result[7].ToString();

                lst.Add(datos);
            }
            result.Close();
            conn.Close();
            ViewBag.lista = lst;

            return View() ;
        }
    }
}
