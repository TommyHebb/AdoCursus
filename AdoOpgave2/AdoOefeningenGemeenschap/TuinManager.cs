using System;
using System.Data;

namespace AdoOefeningenGemeenschap
{
    public class TuinManager
    {
        public void LeverancierToevoegen(Leverancier eenLeverancier)
        {
            using (var conTuin = new TuinDbManager().GetConnection())
            {
                using (var comToevoegen = conTuin.CreateCommand())
                {
                    comToevoegen.CommandType = CommandType.StoredProcedure;
                    comToevoegen.CommandText = "LeverancierToevoegen";
                    var parNaam = comToevoegen.CreateParameter();
                    parNaam.ParameterName = "@Naam";
                    parNaam.Value = eenLeverancier.Naam;
                    comToevoegen.Parameters.Add(parNaam);
                    var parAdres = comToevoegen.CreateParameter();
                    parAdres.ParameterName = "@Adres";
                    parAdres.Value = eenLeverancier.Adres;
                    comToevoegen.Parameters.Add(parAdres);
                    var parPostNr = comToevoegen.CreateParameter();
                    parPostNr.ParameterName = "@PostNr";
                    parPostNr.Value = eenLeverancier.PostNr;
                    comToevoegen.Parameters.Add(parPostNr);
                    var parWoonplaats = comToevoegen.CreateParameter();
                    parWoonplaats.ParameterName = "@Woonplaats";
                    parWoonplaats.Value = eenLeverancier.Woonplaats;
                    comToevoegen.Parameters.Add(parWoonplaats);
                    conTuin.Open();
                    comToevoegen.ExecuteNonQuery();
                }
            }
        }
        public int EindejaarsKorting()
        {
            using (var conTuin = new TuinDbManager().GetConnection())
            {
                using (var comKorting = conTuin.CreateCommand())
                {
                    comKorting.CommandType = CommandType.StoredProcedure;
                    comKorting.CommandText = "EindejaarsKorting";
                    conTuin.Open();
                    return comKorting.ExecuteNonQuery();
                }
            }
        }
        public void VervangLeverancier(int oudLevNr, int nieuwLevNr)
        {
            var manager = new TuinDbManager();
            using (var conTuin = manager.GetConnection())
            {
                conTuin.Open();
                using (var traVervangen = conTuin.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (var comWijzigen = conTuin.CreateCommand())
                    {
                        comWijzigen.Transaction = traVervangen;
                        comWijzigen.CommandType = CommandType.StoredProcedure;
                        comWijzigen.CommandText = "LeverancierWijzigen";
                        var parOudLevNr = comWijzigen.CreateParameter();
                        parOudLevNr.ParameterName = "@OudLevNr";
                        parOudLevNr.Value = oudLevNr;
                        comWijzigen.Parameters.Add(parOudLevNr);
                        var parNieuwLevNr = comWijzigen.CreateParameter();
                        parNieuwLevNr.ParameterName = "@NieuwLevNr";
                        parNieuwLevNr.Value = nieuwLevNr;
                        comWijzigen.Parameters.Add(parNieuwLevNr);
                        if (comWijzigen.ExecuteNonQuery() == 0)
                        {
                            traVervangen.Rollback();
                            throw new Exception("Leverancier " + oudLevNr + 
                                " kon niet vervangen worden door " + nieuwLevNr);
                        }
                    }
                    using (var comVerwijderen = conTuin.CreateCommand())
                    {
                        comVerwijderen.Transaction = traVervangen;
                        comVerwijderen.CommandType = CommandType.StoredProcedure;
                        comVerwijderen.CommandText = "LeverancierVerwijderen";
                        var parLevNr = comVerwijderen.CreateParameter();
                        parLevNr.ParameterName = "@LevNr";
                        parLevNr.Value = oudLevNr;
                        comVerwijderen.Parameters.Add(parLevNr);
                        if (comVerwijderen.ExecuteNonQuery() == 0)
                        {
                            traVervangen.Rollback();
                            throw new Exception("Leverancier " + oudLevNr + 
                                " kon niet verwijderd worden");
                        }
                        traVervangen.Commit();
                    }
                }
            }
        }
        public decimal GemiddeldePrijsVanEenSoort(string soort)
        {
            var manager = new TuinDbManager();
            using (var conTuin = manager.GetConnection())
            {
                using (var comGemiddelde = conTuin.CreateCommand())
                {
                    comGemiddelde.CommandType = CommandType.StoredProcedure;
                    comGemiddelde.CommandText = "GemiddeldePrijsVanEenSoort";
                    var parSoort = comGemiddelde.CreateParameter();
                    parSoort.ParameterName = "@soort";
                    parSoort.Value = soort;
                    comGemiddelde.Parameters.Add(parSoort);
                    conTuin.Open();
                    var resultaat = comGemiddelde.ExecuteScalar();
                    if (resultaat == DBNull.Value)
                    {
                        throw new Exception("Soort bestaat niet");
                    }
                    else
                    {
                        return (decimal)resultaat;
                    }
                }
            }
        }
    }
}
