﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ShoeStoreApp.Models
{
    public class DeliveryAddress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
