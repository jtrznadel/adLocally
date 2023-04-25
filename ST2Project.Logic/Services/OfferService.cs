using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST2Project.Logic.Entities;

namespace ST2Project.Logic.Services
{
    public class OfferService
    {
        st2databaseEntities2 _db = new st2databaseEntities2();
        public List<Offers> GetAllOffers()
        {
            return _db.Offers.ToList();
        }

        public List<Offers> GetSpecifiedOffers(string value)
        {
            return _db.Offers.Where(x => x.Title.Contains(value) ||  x.Description.Contains(value)).ToList();
        }

        public void CreateOffer(Offers _offer)
        {
            try
            {
                _db.Offers.Add(_offer);
                _db.SaveChanges();
            }
                
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }

        public List<Offers> GetUsersOffers(int id) {
            return _db.Offers.Where(x => x.OwnerID == id).ToList();
        }

        public Offers GetSingleOffer(int id){
            return _db.Offers.Where(x => x.OfferID == id).FirstOrDefault();
        }

        public void RemoveOffer(int id)
        {
                var offerToRemove = _db.Offers.Where(x => x.OfferID == id).FirstOrDefault();
                _db.Offers.Remove(offerToRemove);
                _db.SaveChanges();
        }
        public void EditOffer(Offers _offer)
        {
                var offerToEdit = _db.Offers.Find(_offer.OfferID);
                _db.Entry(offerToEdit).CurrentValues.SetValues(_offer);
                _db.SaveChanges();
        }
    }
}
