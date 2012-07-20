﻿// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Topshelf.ServiceConfigurators
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Configurators;
    using Runtime;

    public class DelegateServiceConfigurator<T> :
        ServiceConfigurator<T>
        where T : class
    {
        Func<T, HostControl, bool> _continue;
        ServiceFactory<T> _factory;
        Func<T, HostControl, bool> _pause;
        Func<T, HostControl, bool> _start;
        Func<T, HostControl, bool> _stop;

        public IEnumerable<ValidateResult> Validate()
        {
            if (_factory == null)
                yield return this.Failure("Factory", "must not be null");

            if (_start == null)
                yield return this.Failure("Start", "must not be null");
            if (_stop == null)
                yield return this.Failure("Stop", "must not be null");
            if (_pause == null)
                yield return this.Failure("Pause", "must not be null");
            if (_continue == null)
                yield return this.Failure("Continue", "must not be null");
        }

        public void ConstructUsing(ServiceFactory<T> serviceFactory)
        {
            _factory = serviceFactory;
        }

        public void WhenStarted(Func<T, HostControl, bool> start)
        {
            _start = start;
        }

        public void WhenStopped(Func<T, HostControl, bool> stop)
        {
            _stop = stop;
        }

        public void WhenPaused(Func<T, HostControl, bool> pause)
        {
            _pause = pause;
        }

        public void WhenContinued(Func<T, HostControl, bool> @continue)
        {
            _continue = @continue;
        }

        public ServiceBuilder Build()
        {
            var serviceBuilder = new DelegateServiceBuilder<T>(_factory, _start, _stop, _pause, _continue);
            return serviceBuilder;
        }
    }
}