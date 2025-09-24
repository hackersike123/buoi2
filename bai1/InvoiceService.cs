using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bai1
{
    internal static class InvoiceService
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "invoices.txt");

        public static List<Invoice> LoadInvoices()
        {
            try
            {
                if (!File.Exists(FilePath)) return new List<Invoice>();
                var lines = File.ReadAllLines(FilePath, Encoding.UTF8);
                var list = new List<Invoice>();
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { '|' }, StringSplitOptions.None);
                    if (parts.Length >= 7)
                    {
                        long total = 0;
                        long.TryParse(parts[6], out total);
                        list.Add(new Invoice
                        {
                            Name = parts[0],
                            CCCD = parts[1],
                            Gender = parts[2],
                            Phone = parts[3],
                            Province = parts[4],
                            Seats = parts[5],
                            Total = total
                        });
                    }
                }
                return list;
            }
            catch
            {
                return new List<Invoice>();
            }
        }

        public static void SaveInvoices(List<Invoice> invoices)
        {
            var sb = new StringBuilder();
            foreach (var inv in invoices)
            {
                sb.AppendLine($"{Escape(inv.Name)}|{Escape(inv.CCCD)}|{Escape(inv.Gender)}|{Escape(inv.Phone)}|{Escape(inv.Province)}|{Escape(inv.Seats)}|{inv.Total}");
            }
            File.WriteAllText(FilePath, sb.ToString(), Encoding.UTF8);
        }

        private static string Escape(string s)
        {
            return (s ?? string.Empty).Replace("|", "\\|");
        }
    }
}
