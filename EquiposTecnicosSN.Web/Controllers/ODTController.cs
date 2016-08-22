﻿using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Controllers
{
    /// <summary>
    /// Web Controller Base para ordenes de trabajo
    /// </summary>
    public abstract class ODTController : Controller
    {
        /// <summary>
        /// DbContext de equipo
        /// </summary>
        protected EquiposDbContext db = new EquiposDbContext();
        /// <summary>
        /// DbContext de usuarios
        /// </summary>
        protected IdentityDb usuariosdb = new IdentityDb();

        /// <summary>
        /// Acción que carga los datos de una orden de trabajo.
        /// </summary>
        /// <param name="id">Id de la orden de trabajo</param>
        /// <returns></returns>
        abstract public ActionResult Details(int? id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult CreateForEquipo(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prioridad"></param>
        /// <returns></returns>
        abstract public ActionResult CountOrdenesPrioridad(String prioridad);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult EditGastos(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult Close(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddGasto()
        {
            return PartialView("_AddGastoToOrden", new GastoOrdenDeTrabajo());
        }

        /// <summary>
        /// Guarda los gastos asociados a una orden de trabajo.
        /// </summary>
        /// <param name="gastos">Lista de gastos a presistir</param>
        /// <param name="ordenDeTrabajoId">Id de la orden de trabajo</param>
        protected void SaveGastos(IEnumerable<GastoOrdenDeTrabajo> gastos, int ordenDeTrabajoId)
        {
            var gastosEntidad = db.GastosOrdenesDeTrabajo.Where(g => g.OrdenDeTrabajoId == ordenDeTrabajoId).ToList();

            foreach (var gastoE in gastosEntidad)
            {
                if (gastos.Any(g => g.GastoOrdenDeTrabajoId == gastoE.GastoOrdenDeTrabajoId))
                {
                    var edicion = gastos.Where(g => g.GastoOrdenDeTrabajoId == gastoE.GastoOrdenDeTrabajoId).Single();
                    gastoE.Monto = edicion.Monto;
                    gastoE.Concepto = edicion.Concepto;
                    db.Entry(gastoE).State = EntityState.Modified;
                }
                else
                {
                    db.GastosOrdenesDeTrabajo.Remove(gastoE);
                }
            }

            if (gastos != null)
            {
                var nuevosGastos = gastos.Where(g => g.GastoOrdenDeTrabajoId == 0);
                if (nuevosGastos != null && nuevosGastos.Count() > 0)
                {
                    nuevosGastos.ToList().ForEach(g =>
                    {
                        g.OrdenDeTrabajoId = ordenDeTrabajoId;
                        db.GastosOrdenesDeTrabajo.Add(g);
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuevaObservacion"></param>
        /// <param name="odt"></param>
        protected void SaveNuevaObservacion(ObservacionOrdenDeTrabajo nuevaObservacion, OrdenDeTrabajo odt)
        {
            if (nuevaObservacion.Observacion != null)
            {
                if (odt.Observaciones == null)
                {
                    odt.Observaciones = new List<ObservacionOrdenDeTrabajo>();
                }

                odt.Observaciones.Add(nuevaObservacion);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orden"></param>
        public void CloseSolicitudesRepuestos(OrdenDeTrabajo orden)
        {
            foreach (var s in orden.SolicitudesRespuestos)
            {
                if (s.FechaCierre == null)
                {
                    s.FechaCierre = DateTime.Now;
                    db.Entry(s).State = EntityState.Modified;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ObservacionOrdenDeTrabajo NuevaObservacion()
        {
            return new ObservacionOrdenDeTrabajo
            {
                Fecha = DateTime.Now,
                UsuarioId = 1 //TODO: hardcode
            };
        }
    }
}
