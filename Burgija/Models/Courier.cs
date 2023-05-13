using System;
using System.Collections.Generic;

namespace Burgija.Models
{
    public class Courier : User
    {
        #region Properties

        public List<Delivery> DeliveryRequests { get; set; }
        public List<Delivery> AcceptedRequests { get; set; }

        #endregion

        #region Constructors

        public Courier(int id, string username, string name, string email, string password, List<Delivery> deliveryRequests, List<Delivery> acceptedRequests)
        {
            Id = id;
            Username = username;
            Name = name;
            Email = email;
            Password = password;
            DeliveryRequests = deliveryRequests;
            AcceptedRequests = acceptedRequests;
        }

        public Courier() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}