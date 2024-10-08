﻿using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class DigitalWalletResponse : BaseResponse
    {
        public decimal? PointBalance { get; set; }
        public long UserId { get; set; }
    }
}
