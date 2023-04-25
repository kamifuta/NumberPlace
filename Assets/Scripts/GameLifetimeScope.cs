using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private SquareGenerator squareGenerator;

    [SerializeField] private NumberView numberView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);

        builder.RegisterEntryPoint<NumberPresenter>();

        builder.Register<NumberManager>(Lifetime.Singleton);
        builder.Register<SquareManager>(Lifetime.Singleton).WithParameter("squareGenerator", squareGenerator);

        builder.RegisterComponent(numberView);
    }
}
