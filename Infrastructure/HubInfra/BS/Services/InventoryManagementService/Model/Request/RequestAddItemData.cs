using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Request
{
    public class RequestAddItemData
    {
        public List<ItemsDetail> Items { get; set; } = new List<ItemsDetail>();
    }

    public class ItemsDetail
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long ItmsGrpCod { get; set; }
        public Item PrchseItem { get; set; }
        public Item SellItem { get; set; }
        public Item InvntItem { get; set; }
        public long UgpEntry { get; set; }
        public BuyUnitMsr? BuyUnitMsr { get; set; }
        public BuyUnitMsr? SalUnitMsr { get; set; }
        public BuyUnitMsr? InvntryUom { get; set; }
        public long? PUoMEntry { get; set; }
        public long? SUoMEntry { get; set; }
        public long? IUoMEntry { get; set; }
    }

    public enum BuyUnitMsr { Bag, BuyUnitMsrManual, BuyUnitMsrPiece, Capsules, Ch, Each, Empty, Kilogram, Manual, Menual, Ml, Pc, Pcs, Piece, The100Gram, The361Piece, The50Gram, The721Piece };

    public enum Item { N, Y };
}
