using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.TrainerViewModel;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone)) return false;
                var trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Gender = createTrainer.Gender,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City
                    },
                    Specialties = createTrainer.Specialties
                };
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];
            return trainers.Select(t => new TrainerViewModel
            {
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties.ToString(),
            });
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;
            return new TrainerViewModel
            {
                Name = trainer.Name,
                Specialties = trainer.Specialties.ToString(),
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}"   
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;
            return new TrainerToUpdateViewModel
            {
                Email = trainer.Email,
                Name = trainer.Name,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
            };
        }

        public bool RemoveTrainer(int trainerId)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = TrainerRepo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            TrainerRepo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {
            try
            {
                if (IsEmailExists(updatedTrainer.Email) || IsPhoneExists(updatedTrainer.Phone)) return false;
                var trainerRepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = trainerRepo.GetById(trainerId);
                if (trainer is null) return false;
                trainer.Email = updatedTrainer.Email;
                trainer.Phone = updatedTrainer.Phone;
                trainer.Address.City = updatedTrainer.City;
                trainer.Address.Street = updatedTrainer.Street;
                trainer.Address.BuildingNumber = updatedTrainer.BuildingNumber;
                trainer.UpdatedAt = DateTime.Now;
                trainerRepo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Method
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == phone).Any();
        }
        private bool HasActiveSessions(int Id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(
               s => s.TrainerId == Id && s.StartDate > DateTime.Now).Any();
            return activeSessions;
        }
        #endregion
    }
}
