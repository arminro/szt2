// <copyright file="CentralHub.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace EntryPoint
{
    using System;
    using System.Collections.Generic;
    using Core;
    using DatabaseOperations.Interfaces;
    using Utils.CommonInterfaces;

    /// <summary>
    /// Factory class implementing <see cref="ILogicFactory"/>
    /// </summary>
    public class CentralHub : ILogicFactory
    {
        private UserFunctions logic;
        private Dictionary<Type, IElectroLogicProvider> mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentralHub"/> class.
        /// </summary>
        public CentralHub()
        {
            this.ElectroRepository = new DatabaseOperations.DBCallProcessor();
            this.AdminRepo = new DatabaseOperations.AdministrationsDBCallProcessor();
            this.Session = new Authentication.SessionManager(this.ElectroRepository, this.AdminRepo);
            this.Messenger = new Messaging.NotificationManager(this.Session);
            this.logic = new UserFunctions(this.ElectroRepository, this.AdminRepo, this.Session, this.Messenger);
            this.mapper = new Dictionary<Type, IElectroLogicProvider>();
            this.MapperSetup();
        }

        /// <inheritdoc/>
        public ISession Session { get;  }

        private IMessenger Messenger { get; set; }

        private IElectroStoreRepository ElectroRepository { get; set; }

        private IAdministratorRepository AdminRepo { get; set; }

        /// <summary>
        /// Gets the logic of the specified type that must be an IElectrologicProvider
        /// </summary>
        /// <typeparam name="T">The type of a logic</typeparam>
        /// <returns>A logic handler instance of type T</returns>
        public T GetLogic<T>()
            where T : IElectroLogicProvider
        {
            if (this.mapper.ContainsKey(typeof(T)))
            {
                return (T)this.mapper[typeof(T)];
            }
            else
            {
                throw new ApplicationException($"{typeof(T).Name} is not added to the internal dictioanary, therefore, it cannot be used as of now");
            }
        }

        private void MapperSetup()
        {
            if (this.mapper != null)
            {
                this.mapper.Add(typeof(IClerk), this.logic as IClerk);
                this.mapper.Add(typeof(IMainClerk), this.logic as IMainClerk);
                this.mapper.Add(typeof(IAdmin), this.logic as IAdmin);
            }
            else
            {
                throw new ApplicationException("The internal typemap is not instantiated");
            }
        }
    }
}
