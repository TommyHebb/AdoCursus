using System;
using System.Data;
using System.Data.Common;

namespace AdoGemeenschap
{
    public class RekeningenManager
    {
        public Int32 SaldoBonus()
        {
            var dbManager = new BankDbManager();
            using (var conBank = dbManager.GetConnection())
            {
                using (var comBonus = conBank.CreateCommand())
                {
                    comBonus.CommandType = CommandType.Text;
                    comBonus.CommandText = "update Rekeningen set Saldo=Saldo*1.1";
                    conBank.Open();
                    return comBonus.ExecuteNonQuery();
                }
            }
        }
        public Boolean Storten(Decimal teStorten, String rekeningNr)
        {
            BankDbManager dbManager = new BankDbManager();
            using (var conBank = dbManager.GetConnection())
            {
                using (var comStorten = conBank.CreateCommand())
                {
                    comStorten.CommandType = CommandType.StoredProcedure;
                    comStorten.CommandText = "Storten";
                    DbParameter parTeStorten = comStorten.CreateParameter();
                    parTeStorten.ParameterName = "@teStorten";
                    parTeStorten.Value = teStorten;
                    parTeStorten.DbType = DbType.Currency;
                    comStorten.Parameters.Add(parTeStorten);
                    DbParameter parRekeningNr = comStorten.CreateParameter();
                    parRekeningNr.ParameterName = "@RekeningNr";
                    parRekeningNr.Value = rekeningNr;
                    comStorten.Parameters.Add(parRekeningNr);
                    conBank.Open();
                    return comStorten.ExecuteNonQuery() != 0;
                }
            }
        }
        public void Overschrijven(Decimal bedrag, String vanRekening, String naarRekening)
        {
            using (var conBank = new BankDbManager().GetConnection())
            {
                conBank.Open();
                using (var traOverschrijven = conBank.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (var comAftrekken = conBank.CreateCommand())
                    {
                        comAftrekken.Transaction = traOverschrijven;
                        comAftrekken.CommandType = CommandType.Text;
                        comAftrekken.CommandText = 
                            "update Rekeningen set Saldo=Saldo-@bedrag where RekeningNr = @reknr";
                        var parBedrag = comAftrekken.CreateParameter();
                        parBedrag.ParameterName = "@bedrag";
                        parBedrag.Value = bedrag;
                        comAftrekken.Parameters.Add(parBedrag);
                        var parRekNr = comAftrekken.CreateParameter();
                        parRekNr.ParameterName = "@reknr";
                        parRekNr.Value = vanRekening;
                        comAftrekken.Parameters.Add(parRekNr);
                        if (comAftrekken.ExecuteNonQuery() == 0)
                        {
                            traOverschrijven.Rollback();
                            throw new Exception("Van rekening bestaat niet");
                        }
                    } // using comAftrekken
                    using (var comBijtellen = conBank.CreateCommand())
                    {
                        comBijtellen.Transaction = traOverschrijven;
                        comBijtellen.CommandType = CommandType.Text;
                        comBijtellen.CommandText = 
                            "update Rekeningen set Saldo = Saldo + @bedrag where RekeningNr = @reknr";
                        var parBedrag = comBijtellen.CreateParameter();
                        parBedrag.ParameterName = "@bedrag";
                        parBedrag.Value = bedrag;
                        comBijtellen.Parameters.Add(parBedrag);
                        var parRekNr = comBijtellen.CreateParameter();
                        parRekNr.ParameterName = "@reknr";
                        parRekNr.Value = naarRekening;
                        comBijtellen.Parameters.Add(parRekNr);
                        if (comBijtellen.ExecuteNonQuery() == 0)
                        {
                            traOverschrijven.Rollback();
                            throw new Exception("Naar rekening bestaat niet");
                        }
                    } // using comBijtellen
                    traOverschrijven.Commit();
                } // using traOverschrijven
            } // using conBank
        }
    }
}
