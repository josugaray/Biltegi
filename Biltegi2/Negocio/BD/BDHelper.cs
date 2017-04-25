using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;

namespace Biltegi2.Negocio.BD
{
   
    public class BDHelper : SQLiteOpenHelper
    {
        // specifies the database name
        private const string DatabaseName = "BiltegiBD";
        //specifies the database version (increment this number each time you make database related changes)
        private const int DatabaseVersion = 1;

        public BDHelper(Context context)
            : base(context, DatabaseName, null, DatabaseVersion)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            //create database tables
            db.ExecSQL(@"
                    CREATE TABLE IF NOT EXISTS GRUPO (
                        IdGrupo              INTEGER PRIMARY KEY AUTOINCREMENT,
                        Descripcion       TEXT NOT NULL )");
            db.ExecSQL(@"
                    CREATE TABLE IF NOT EXISTS ELEMENTO (
                        IdElemento              INTEGER PRIMARY KEY AUTOINCREMENT,
                        Descripcion       TEXT NOT NULL,
                        CodigoBarras    TEXT,
                        IdProducto     INTEGER)");
            db.ExecSQL(@"
                    CREATE TABLE IF NOT EXISTS PRODUCTO (
                        IdProducto              INTEGER PRIMARY KEY AUTOINCREMENT,
                        Descripcion       TEXT NOT NULL,
                        IdGrupo     INTEGER)");

            db.ExecSQL(@"
                    CREATE TABLE IF NOT EXISTS EXISTENCIAS (
                        IdExistencias        INTEGER PRIMARY KEY AUTOINCREMENT,
                        IdElemento              INTEGER,
                        FechaEntrada       DATE_TIME,
                        Cantidad            INTEGER,
                        FechaCaducidad  DATE_TIME)");
            //create database indexes
            //db.ExecSQL(@"CREATE INDEX IF NOT EXISTS FIRSTNAME_CUSTOMER ON CUSTOMER (FIRSTNAME)");
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            if (oldVersion < 2)
            {
                //perform any database upgrade tasks for versions prior to  version 2              
            }
            if (oldVersion < 3)
            {
                //perform any database upgrade tasks for versions prior to  version 3
            }
        }
    }

    
}