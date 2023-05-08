using System;

namespace Burgija.Models
{
    public class Courier : User
    {
        #region Attributes

        private List<Delivery> deliveryRequests;
        private List<Delivery> acceptedRequests;

        #endregion

        #region Properties

        public List<Delivery> DeliveryRequests { get => deliveryRequests; set => deliveryRequests = value; }
        public List<Delivery> AcceptedRequests { get => acceptedRequests; set => acceptedRequests = value; }

        #endregion

        #region Constructor

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

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}