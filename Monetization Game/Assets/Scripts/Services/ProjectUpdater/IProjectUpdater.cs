using System;

namespace Services.ProjectUpdater
{
    public interface IProjectUpdater
    {
        event Action UpdateCalled;
        event Action FixedUpdateCalled;
        event Action LateUpdateCalled;
    }
}