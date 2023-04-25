using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class NumberManager
{
    private NumberGenerator numberGenerator;

    public int[,][,] numbers { get; private set; }

    private ISubject<int[,][,]> numberSubject = new Subject<int[,][,]>();
    public IObservable<int[,][,]> NumberObservable => numberSubject;

    [Inject]
    public NumberManager()
    {
        numberGenerator = new NumberGenerator();
    }

    public void GenerateNumbers()
    {
        numbers = numberGenerator.GenerateNumbers();
        numberSubject.OnNext(numbers);
    }
}
