using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRDLCSub.Blazor.Data
{
    public class EmpleadoService
    {
        public DataTable GetEmpleado()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Departamento");
            dt.Columns.Add("FechaInicio");
            DataRow dr;
            for (int i = 1; i < 50; i++)
            {
                dr = dt.NewRow();
                dr["Id"] = i;
                dr["Nombre"] = "Empleado - " + i;
                dr["Departamento"] = "Desarrollo";
                dr["FechaInicio"] = DateTime.Now.AddYears(-1).AddDays(i);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable GetEmpleadoDetalles()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Detalle");
            DataRow row;
            for (int i = 1; i < 50; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    row = dt.NewRow();
                    row["Id"] = i;
                    row["Detalle"] = "Detalle - " + j;
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }
    }
}
