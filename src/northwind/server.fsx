#if INTERACTIVE
#I "../../packages"
#r "System.Xml.Linq.dll"
#r "FSharp.Data/lib/net40/FSharp.Data.dll"
#r "Newtonsoft.Json/lib/net40/Newtonsoft.Json.dll"
#r "Suave/lib/net40/Suave.dll"
#r "SQLProvider/lib/FSharp.Data.SqlProvider.dll"
#r "Mono.Data.Sqlite.Portable/lib/net4/Mono.Data.Sqlite.dll"
#load "../serializer.fs"
#load "../facet.fs"
#else

module Services.Northwind
#endif
#nowarn "1104"
open System
open System.IO
open FSharp.Data
open FSharp.Data.Sql
open System.Collections.Generic
open Services.Serializer
open Services.Facets


// ----------------------------------------------------------------------------
// Server
// ----------------------------------------------------------------------------

[<Literal>]
let dataPath = __SOURCE_DIRECTORY__  + "/../../data"

[<Literal>]
let resolutionPath = __SOURCE_DIRECTORY__ + @"/../../packages/Mono.Data.Sqlite.Portable/lib/net4/"

[<Literal>]
let connectionString = "Data Source=" + dataPath + @"/northwindEF.db;Version=3;Read Only=false;FailIfMissing=True;"

type sql = SqlDataProvider<
              ConnectionString = connectionString,
              DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
              CaseSensitivityChange=Common.CaseSensitivityChange.ORIGINAL,
              ResolutionPath = resolutionPath,
              IndividualsAmount = 1000,
              UseOptionTypes = true>

let ctx = sql.GetDataContext()

let employees =
    ctx.Main.Employees
    |> Seq.map(fun e -> e.EmployeeId, e.LastName + ", " + e.FirstName)
    |> dict

let customers =
    ctx.Main.Customers
    |> Seq.map(fun c -> c.CustomerId, c.CompanyName)
    |> dict

