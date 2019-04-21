// <copyright file="ILogicFactory.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils.CommonInterfaces
{
    /// <summary>
    /// Provides a means to hide implementation of logic interfaces
    /// </summary>
    public interface ILogicFactory
    {
        /// <summary>
        /// Gets the session object of the current session
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Get a logic of T
        /// </summary>
        /// <typeparam name="T">A logic type that must be an IElectroLogicProvider</typeparam>
        /// <returns>Return the logic implementation of the specified interface type</returns>
        T GetLogic<T>()
            where T : IElectroLogicProvider;
    }
}
