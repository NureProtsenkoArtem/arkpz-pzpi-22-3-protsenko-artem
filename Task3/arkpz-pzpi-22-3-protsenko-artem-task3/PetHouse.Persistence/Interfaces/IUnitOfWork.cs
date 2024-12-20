﻿namespace PetHouse.Persistence.Interfaces;

public interface IUnitOfWork
{
   IRepository<T> Repository<T>() where T : class;
   Task SaveChangesAsync();
   
}