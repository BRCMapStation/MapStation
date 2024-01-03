using System;

namespace Reptile {
    // Token: 0x020002D3 RID: 723
    [Serializable]
    public enum AudioClipID {
        // Token: 0x040018C4 RID: 6340
        NONE = -1,
        // Token: 0x040018C5 RID: 6341
        Downhilll_Ambience = 587,
        // Token: 0x040018C6 RID: 6342
        Dream_Ambience,
        // Token: 0x040018C7 RID: 6343
        Escalator_Ambience,
        // Token: 0x040018C8 RID: 6344
        Mall_Ambience,
        // Token: 0x040018C9 RID: 6345
        Osaka_Ambience,
        // Token: 0x040018CA RID: 6346
        Prelude_Ambience_Cell = 599,
        // Token: 0x040018CB RID: 6347
        Prelude_Ambience_Outside = 598,
        // Token: 0x040018CC RID: 6348
        Prelude_Ambience_StationInterior = 597,
        // Token: 0x040018CD RID: 6349
        Pyramid_Ambience = 592,
        // Token: 0x040018CE RID: 6350
        Tower_Port_Ambience,
        // Token: 0x040018CF RID: 6351
        Waterfall_Ambience,
        // Token: 0x040018D0 RID: 6352
        Wind_Ambience,
        // Token: 0x040018D1 RID: 6353
        cop_jetpack_loop = 286,
        // Token: 0x040018D2 RID: 6354
        cop_jetpack_start,
        // Token: 0x040018D3 RID: 6355
        deployCanister = 30,
        // Token: 0x040018D4 RID: 6356
        policeAlarm = 36,
        // Token: 0x040018D5 RID: 6357
        policeWallDown = 128,
        // Token: 0x040018D6 RID: 6358
        policeWallUp,
        // Token: 0x040018D7 RID: 6359
        recordWoosh = 276,
        // Token: 0x040018D8 RID: 6360
        tankwalkerFootstep01 = 289,
        // Token: 0x040018D9 RID: 6361
        BatonAttackHit = 344,
        // Token: 0x040018DA RID: 6362
        BodyDrop = 359,
        // Token: 0x040018DB RID: 6363
        BulletHit,
        // Token: 0x040018DC RID: 6364
        BulletRicochet = 330,
        // Token: 0x040018DD RID: 6365
        chainBreak = 459,
        // Token: 0x040018DE RID: 6366
        ClawAttackHit = 346,
        // Token: 0x040018DF RID: 6367
        CockpitGetHit = 456,
        // Token: 0x040018E0 RID: 6368
        CopterBlades = 354,
        // Token: 0x040018E1 RID: 6369
        CopterBladesCrash,
        // Token: 0x040018E2 RID: 6370
        CopterMachinegunShot = 334,
        // Token: 0x040018E3 RID: 6371
        cuffClose = 454,
        // Token: 0x040018E4 RID: 6372
        cuffMissile,
        // Token: 0x040018E5 RID: 6373
        DJJump = 517,
        // Token: 0x040018E6 RID: 6374
        DJLand,
        // Token: 0x040018E7 RID: 6375
        DJOpen,
        // Token: 0x040018E8 RID: 6376
        DJVinylCounter,
        // Token: 0x040018E9 RID: 6377
        DJVinylThrow,
        // Token: 0x040018EA RID: 6378
        DJWalking,
        // Token: 0x040018EB RID: 6379
        EnemyGetHit = 347,
        // Token: 0x040018EC RID: 6380
        FemaleDeath = 351,
        // Token: 0x040018ED RID: 6381
        FemaleHurt = 350,
        // Token: 0x040018EE RID: 6382
        HatchOpen = 356,
        // Token: 0x040018EF RID: 6383
        MachineryExplosion = 358,
        // Token: 0x040018F0 RID: 6384
        Male2Attack = 525,
        // Token: 0x040018F1 RID: 6385
        Male2Hurt = 524,
        // Token: 0x040018F2 RID: 6386
        MaleAttack = 523,
        // Token: 0x040018F3 RID: 6387
        MaleDeath = 348,
        // Token: 0x040018F4 RID: 6388
        MaleHurt,
        // Token: 0x040018F5 RID: 6389
        MortarExplosion = 357,
        // Token: 0x040018F6 RID: 6390
        PistolReload = 339,
        // Token: 0x040018F7 RID: 6391
        PistolShot = 335,
        // Token: 0x040018F8 RID: 6392
        PoliceTubeMoveDown = 362,
        // Token: 0x040018F9 RID: 6393
        PoliceTubeMoveUp = 361,
        // Token: 0x040018FA RID: 6394
        ShieldAttackHit = 345,
        // Token: 0x040018FB RID: 6395
        ShieldBlock = 352,
        // Token: 0x040018FC RID: 6396
        SniperReload = 340,
        // Token: 0x040018FD RID: 6397
        SniperShot = 338,
        // Token: 0x040018FE RID: 6398
        sniper_lockon = 119,
        // Token: 0x040018FF RID: 6399
        TankWalkerGunAway = 465,
        // Token: 0x04001900 RID: 6400
        TankWalkerGunStart,
        // Token: 0x04001901 RID: 6401
        TankwalkerHeadStab = 331,
        // Token: 0x04001902 RID: 6402
        TankwalkerMachinegunShot = 336,
        // Token: 0x04001903 RID: 6403
        TankwalkerMortarShot,
        // Token: 0x04001904 RID: 6404
        TankwalkerPound = 332,
        // Token: 0x04001905 RID: 6405
        TankwalkerStomp,
        // Token: 0x04001906 RID: 6406
        tankwalkerVent = 288,
        // Token: 0x04001907 RID: 6407
        TankWalkerWindUp = 464,
        // Token: 0x04001908 RID: 6408
        TurretDown = 458,
        // Token: 0x04001909 RID: 6409
        TurretRaise = 457,
        // Token: 0x0400190A RID: 6410
        car_engine = 55,
        // Token: 0x0400190B RID: 6411
        car_hood_impact = 27,
        // Token: 0x0400190C RID: 6412
        vendingMachine = 542,
        // Token: 0x0400190D RID: 6413
        vendingMachine_drop = 546,
        // Token: 0x0400190E RID: 6414
        vendingMachine_short = 541,
        // Token: 0x0400190F RID: 6415
        Basketball = 375,
        // Token: 0x04001910 RID: 6416
        Bike,
        // Token: 0x04001911 RID: 6417
        BMXGatewayApproved = 493,
        // Token: 0x04001912 RID: 6418
        BMXGatewayChecking,
        // Token: 0x04001913 RID: 6419
        BMXGatewayDenied,
        // Token: 0x04001914 RID: 6420
        BoostGlassDoorBounce = 509,
        // Token: 0x04001915 RID: 6421
        Bottle = 377,
        // Token: 0x04001916 RID: 6422
        Can,
        // Token: 0x04001917 RID: 6423
        CarBrake,
        // Token: 0x04001918 RID: 6424
        Cardboard,
        // Token: 0x04001919 RID: 6425
        CarHorn,
        // Token: 0x0400191A RID: 6426
        CashRegister = 501,
        // Token: 0x0400191B RID: 6427
        ClothingRack,
        // Token: 0x0400191C RID: 6428
        Crate = 382,
        // Token: 0x0400191D RID: 6429
        DeckProp = 416,
        // Token: 0x0400191E RID: 6430
        Dog = 554,
        // Token: 0x0400191F RID: 6431
        FireExtinguisher = 516,
        // Token: 0x04001920 RID: 6432
        Glass = 383,
        // Token: 0x04001921 RID: 6433
        Guitar = 511,
        // Token: 0x04001922 RID: 6434
        MarketStand = 384,
        // Token: 0x04001923 RID: 6435
        MascotHit = 409,
        // Token: 0x04001924 RID: 6436
        MascotSpin = 510,
        // Token: 0x04001925 RID: 6437
        MonorailDrive = 492,
        // Token: 0x04001926 RID: 6438
        Paper = 503,
        // Token: 0x04001927 RID: 6439
        PlasticSign = 513,
        // Token: 0x04001928 RID: 6440
        PortaPottyClose,
        // Token: 0x04001929 RID: 6441
        PortaPottyOpen,
        // Token: 0x0400192A RID: 6442
        ScrewPoleMoving = 491,
        // Token: 0x0400192B RID: 6443
        ShoppingCart = 414,
        // Token: 0x0400192C RID: 6444
        SkatesProp = 417,
        // Token: 0x0400192D RID: 6445
        SprayCan = 504,
        // Token: 0x0400192E RID: 6446
        StoreShelf = 497,
        // Token: 0x0400192F RID: 6447
        Table = 506,
        // Token: 0x04001930 RID: 6448
        ToiletDoorClose = 499,
        // Token: 0x04001931 RID: 6449
        ToiletDoorOpen,
        // Token: 0x04001932 RID: 6450
        Trash = 385,
        // Token: 0x04001933 RID: 6451
        TrashBag,
        // Token: 0x04001934 RID: 6452
        TrashBin = 415,
        // Token: 0x04001935 RID: 6453
        Trolly = 512,
        // Token: 0x04001936 RID: 6454
        TV = 505,
        // Token: 0x04001937 RID: 6455
        VentEntrance = 412,
        // Token: 0x04001938 RID: 6456
        airdash = 4,
        // Token: 0x04001939 RID: 6457
        boostStart = 528,
        // Token: 0x0400193A RID: 6458
        boostStop = 535,
        // Token: 0x0400193B RID: 6459
        jump_special = 130,
        // Token: 0x0400193C RID: 6460
        launcher_woosh = 114,
        // Token: 0x0400193D RID: 6461
        singleBoost = 19,
        // Token: 0x0400193E RID: 6462
        slideSuperLoop = 52,
        // Token: 0x0400193F RID: 6463
        specialTrick = 326,
        // Token: 0x04001940 RID: 6464
        ultraBoostLoop = 529,
        // Token: 0x04001941 RID: 6465
        slideDropdownBoost = 482,
        // Token: 0x04001942 RID: 6466
        wallrunStart,
        // Token: 0x04001943 RID: 6467
        CanShakeLoop = 301,
        // Token: 0x04001944 RID: 6468
        graffitiComplete = 559,
        // Token: 0x04001945 RID: 6469
        graffitiSlash = 10,
        // Token: 0x04001946 RID: 6470
        Spray = 387,
        // Token: 0x04001947 RID: 6471
        sprayCanPop = 303,
        // Token: 0x04001948 RID: 6472
        grindLoop = 11,
        // Token: 0x04001949 RID: 6473
        railStart = 327,
        // Token: 0x0400194A RID: 6474
        railStop,
        // Token: 0x0400194B RID: 6475
        HollowRail_Grind_Loop_05 = 467,
        // Token: 0x0400194C RID: 6476
        HollowRail_Grind_Loop_06,
        // Token: 0x0400194D RID: 6477
        OldStartStop = 388,
        // Token: 0x0400194E RID: 6478
        cancel = 550,
        // Token: 0x0400194F RID: 6479
        confirm = 547,
        // Token: 0x04001950 RID: 6480
        musicCollectablePickup = 271,
        // Token: 0x04001951 RID: 6481
        pickupCharge = 15,
        // Token: 0x04001952 RID: 6482
        repPickup = 551,
        // Token: 0x04001953 RID: 6483
        selectionMove = 548,
        // Token: 0x04001954 RID: 6484
        TestUISound = 267,
        // Token: 0x04001955 RID: 6485
        ui_phoneBack = 280,
        // Token: 0x04001956 RID: 6486
        ui_phoneConfirm,
        // Token: 0x04001957 RID: 6487
        ui_phoneCursor,
        // Token: 0x04001958 RID: 6488
        ui_phoneFlipClose,
        // Token: 0x04001959 RID: 6489
        ui_phoneFlipOpen,
        // Token: 0x0400195A RID: 6490
        ui_phoneMessage,
        // Token: 0x0400195B RID: 6491
        unlockNotification = 371,
        // Token: 0x0400195C RID: 6492
        handplant = 389,
        // Token: 0x0400195D RID: 6493
        jump = 14,
        // Token: 0x0400195E RID: 6494
        BackFlip_0 = 460,
        // Token: 0x0400195F RID: 6495
        BulletSpin = 507,
        // Token: 0x04001960 RID: 6496
        Cheat720 = 461,
        // Token: 0x04001961 RID: 6497
        footstep = 309,
        // Token: 0x04001962 RID: 6498
        groundTrick = 318,
        // Token: 0x04001963 RID: 6499
        HookKick = 462,
        // Token: 0x04001964 RID: 6500
        land = 255,
        // Token: 0x04001965 RID: 6501
        LegSweep = 463,
        // Token: 0x04001966 RID: 6502
        ScrewPolePlant = 490,
        // Token: 0x04001967 RID: 6503
        slideLoop = 51,
        // Token: 0x04001968 RID: 6504
        AirTime = 390,
        // Token: 0x04001969 RID: 6505
        wallrunLoop = 407,
        // Token: 0x0400196A RID: 6506
        Backflip = 419,
        // Token: 0x0400196B RID: 6507
        GrindTrick_Crank,
        // Token: 0x0400196C RID: 6508
        GrindTrick_ToothPick,
        // Token: 0x0400196D RID: 6509
        GrindTrick_Whiplash,
        // Token: 0x0400196E RID: 6510
        groundTrick3 = 408,
        // Token: 0x0400196F RID: 6511
        groundTrick4 = 413,
        // Token: 0x04001970 RID: 6512
        pedalLeft = 392,
        // Token: 0x04001971 RID: 6513
        PedalLeftShort,
        // Token: 0x04001972 RID: 6514
        pedalRight,
        // Token: 0x04001973 RID: 6515
        PedalRightShort,
        // Token: 0x04001974 RID: 6516
        SeatGrab = 426,
        // Token: 0x04001975 RID: 6517
        TailWhip = 418,
        // Token: 0x04001976 RID: 6518
        moveLoop = 329,
        // Token: 0x04001977 RID: 6519
        backside360varial = 508,
        // Token: 0x04001978 RID: 6520
        groundTrick2 = 405,
        // Token: 0x04001979 RID: 6521
        SBRailStart = 449,
        // Token: 0x0400197A RID: 6522
        Cork720 = 443,
        // Token: 0x0400197B RID: 6523
        DuckWalk = 450,
        // Token: 0x0400197C RID: 6524
        GrindTrick3 = 445,
        // Token: 0x0400197D RID: 6525
        InLineGrindStart,
        // Token: 0x0400197E RID: 6526
        SavCessSlide = 451,
        // Token: 0x0400197F RID: 6527
        StaleGrab = 444,
        // Token: 0x04001980 RID: 6528
        strideLeft = 447,
        // Token: 0x04001981 RID: 6529
        strideRight,
        // Token: 0x04001982 RID: 6530
        UnitySpinCessSlide = 452,
        // Token: 0x04001983 RID: 6531
        FlipPhone_Back = 305,
        // Token: 0x04001984 RID: 6532
        Flipphone_close = 7,
        // Token: 0x04001985 RID: 6533
        FlipPhone_Confirm = 306,
        // Token: 0x04001986 RID: 6534
        Flipphone_open = 8,
        // Token: 0x04001987 RID: 6535
        FlipPhone_RingTone = 307,
        // Token: 0x04001988 RID: 6536
        FlipPhone_Select,
        // Token: 0x04001989 RID: 6537
        beheading = 277,
        // Token: 0x0400198A RID: 6538
        ch1s10DJSquadDrop = 600,
        // Token: 0x0400198B RID: 6539
        ch1s10VinylDrop,
        // Token: 0x0400198C RID: 6540
        ch1s10_DJArrival,
        // Token: 0x0400198D RID: 6541
        ch1s10_DJGetUp,
        // Token: 0x0400198E RID: 6542
        ch1s10_DJOpen,
        // Token: 0x0400198F RID: 6543
        ch1s10_DJVStop,
        // Token: 0x04001990 RID: 6544
        ch1s10_EnterDream,
        // Token: 0x04001991 RID: 6545
        ch1s10_RedRun,
        // Token: 0x04001992 RID: 6546
        ch1s10_SpinAndOpen,
        // Token: 0x04001993 RID: 6547
        ch1s10_SquadWalk = 612,
        // Token: 0x04001994 RID: 6548
        ch1s10_VinylPush = 609,
        // Token: 0x04001995 RID: 6549
        ch1s10_WindUp,
        // Token: 0x04001996 RID: 6550
        Ch1S1a_CellAmb = 571,
        // Token: 0x04001997 RID: 6551
        Ch1S1a_ChiefFS,
        // Token: 0x04001998 RID: 6552
        Ch1S1a_Destruction = 566,
        // Token: 0x04001999 RID: 6553
        Ch1S1a_OutsideStation,
        // Token: 0x0400199A RID: 6554
        Ch1S1a_PoliceRunning,
        // Token: 0x0400199B RID: 6555
        Ch1S1a_ServerRoom,
        // Token: 0x0400199C RID: 6556
        Ch1S1a_StationAmb,
        // Token: 0x0400199D RID: 6557
        ch1s1b_DoorOpen = 573,
        // Token: 0x0400199E RID: 6558
        ch1s1b_lockpick,
        // Token: 0x0400199F RID: 6559
        Ch1s5a_BootUp,
        // Token: 0x040019A0 RID: 6560
        Ch3S4B_GlassBreak = 615,
        // Token: 0x040019A1 RID: 6561
        Ch3S4B_GlassFalling,
        // Token: 0x040019A2 RID: 6562
        Ch3S4B_HeliEngine,
        // Token: 0x040019A3 RID: 6563
        Ch3S4B_HeliPass,
        // Token: 0x040019A4 RID: 6564
        Ch3S4B_SkyAmbience,
        // Token: 0x040019A5 RID: 6565
        Ch3S4B_TankwalkerDrop,
        // Token: 0x040019A6 RID: 6566
        Ch3S4B_TankwalkerTransform,
        // Token: 0x040019A7 RID: 6567
        Ch3S4B_TWDrop,
        // Token: 0x040019A8 RID: 6568
        Ch3S4B_TWVent,
        // Token: 0x040019A9 RID: 6569
        ch4s6b_DreamEntry,
        // Token: 0x040019AA RID: 6570
        CombatEncounterTrap_WallUp = 613,
        // Token: 0x040019AB RID: 6571
        IreneTakeOff_PreludeOutro = 576,
        // Token: 0x040019AC RID: 6572
        siren = 273,
        // Token: 0x040019AD RID: 6573
        WallDownPyramidTrapSequence = 614,
        // Token: 0x040019AE RID: 6574
        cat = 401,
        // Token: 0x040019AF RID: 6575
        dogBark,
        // Token: 0x040019B0 RID: 6576
        femaleRobo,
        // Token: 0x040019B1 RID: 6577
        maleDoeNormaal,
        // Token: 0x040019B2 RID: 6578
        VoiceDie = 484,
        // Token: 0x040019B3 RID: 6579
        VoiceDieFall,
        // Token: 0x040019B4 RID: 6580
        VoiceTalk,
        // Token: 0x040019B5 RID: 6581
        VoiceBoostTrick = 498,
        // Token: 0x040019B6 RID: 6582
        VoiceCombo = 487,
        // Token: 0x040019B7 RID: 6583
        VoiceGetHit,
        // Token: 0x040019B8 RID: 6584
        VoiceJump,
        // Token: 0x040019B9 RID: 6585
        Ch4s2_Tinnitus = 625,
        // Token: 0x040019BA RID: 6586
        Ch4s2_FaceZoom,
        // Token: 0x040019BB RID: 6587
        Ch4s2_Snipershot = 628,
        // Token: 0x040019BC RID: 6588
        Ch4s2_SniperArrival,
        // Token: 0x040019BD RID: 6589
        Ch4s2_Fall,
        // Token: 0x040019BE RID: 6590
        Ch4s2_Blood_1 = 632,
        // Token: 0x040019BF RID: 6591
        Ch4s2_Snipershot2,
        // Token: 0x040019C0 RID: 6592
        ch3s4a_CameraWhoosh,
        // Token: 0x040019C1 RID: 6593
        ch3s4a_BrainWash,
        // Token: 0x040019C2 RID: 6594
        ch3s4a_BatonFall,
        // Token: 0x040019C3 RID: 6595
        ch3s4a_GunDrawCock,
        // Token: 0x040019C4 RID: 6596
        ch3s4a_RadioStatic,
        // Token: 0x040019C5 RID: 6597
        ch3s4a_FSTubes,
        // Token: 0x040019C6 RID: 6598
        Ch3s5_EnterDream,
        // Token: 0x040019C7 RID: 6599
        BasicCopArrivalMall,
        // Token: 0x040019C8 RID: 6600
        TankWalkerDropMall,
        // Token: 0x040019C9 RID: 6601
        TankWalkerDropStand,
        // Token: 0x040019CA RID: 6602
        CopterArriveMall_6 = 649,
        // Token: 0x040019CB RID: 6603
        BasicCopArrivalDry_2,
        // Token: 0x040019CC RID: 6604
        ShieldCopWalkUp_01,
        // Token: 0x040019CD RID: 6605
        ShieldHitArrive_01,
        // Token: 0x040019CE RID: 6606
        SnipersArrive,
        // Token: 0x040019CF RID: 6607
        SniperCopReady,
        // Token: 0x040019D0 RID: 6608
        SniperStand,
        // Token: 0x040019D1 RID: 6609
        TurretHeat,
        // Token: 0x040019D2 RID: 6610
        SniperWhoosh,
        // Token: 0x040019D3 RID: 6611
        TurretBeep_01,
        // Token: 0x040019D4 RID: 6612
        TurretBeep_02,
        // Token: 0x040019D5 RID: 6613
        TankWalkerVictory_Fall,
        // Token: 0x040019D6 RID: 6614
        TankWalkerVictory_HeadSpin,
        // Token: 0x040019D7 RID: 6615
        TankWalkerVictory_FS,
        // Token: 0x040019D8 RID: 6616
        TankWalkerVictory_FS_2,
        // Token: 0x040019D9 RID: 6617
        TankWalkerVictory_FallBarrel,
        // Token: 0x040019DA RID: 6618
        TankWalkerVictory_Fall2,
        // Token: 0x040019DB RID: 6619
        TankWalkerVictory_FallBarrel_2,
        // Token: 0x040019DC RID: 6620
        ch1s4_SlowMo,
        // Token: 0x040019DD RID: 6621
        ch1s4Ambience,
        // Token: 0x040019DE RID: 6622
        ch1s4_DJZoom,
        // Token: 0x040019DF RID: 6623
        ch1s4_Decapitate2,
        // Token: 0x040019E0 RID: 6624
        ch1s4_FauxHand,
        // Token: 0x040019E1 RID: 6625
        ch1s4_HeartBeat,
        // Token: 0x040019E2 RID: 6626
        ch1s4_DiscMovement,
        // Token: 0x040019E3 RID: 6627
        ch1s4_DoorRoof,
        // Token: 0x040019E4 RID: 6628
        ch1s4_DJDisc,
        // Token: 0x040019E5 RID: 6629
        ch1s4_BodyDrop,
        // Token: 0x040019E6 RID: 6630
        ch1s4_Clap,
        // Token: 0x040019E7 RID: 6631
        ch1s4_DJMovement,
        // Token: 0x040019E8 RID: 6632
        ch1s4_DJDecapitate3,
        // Token: 0x040019E9 RID: 6633
        MascotUnlock,
        // Token: 0x040019EA RID: 6634
        ch1s4_Ambience2,
        // Token: 0x040019EB RID: 6635
        TaxiLeave,
        // Token: 0x040019EC RID: 6636
        TaxiArrive,
        // Token: 0x040019ED RID: 6637
        TaxiShowUp,
        // Token: 0x040019EE RID: 6638
        TaxiArrive_02,
        // Token: 0x040019EF RID: 6639
        TaxiCancel,
        // Token: 0x040019F0 RID: 6640
        TaxiArrive_03,
        // Token: 0x040019F1 RID: 6641
        ch5s5_TWGunsOutVerb_2,
        // Token: 0x040019F2 RID: 6642
        ch5s5_TWZoom,
        // Token: 0x040019F3 RID: 6643
        ch5s5_TWOpen,
        // Token: 0x040019F4 RID: 6644
        ch5s5_TWZoomOut,
        // Token: 0x040019F5 RID: 6645
        ch5s5_TWArrive,
        // Token: 0x040019F6 RID: 6646
        ch5s5_TWFinalFS,
        // Token: 0x040019F7 RID: 6647
        ch5s5_RedGetUp,
        // Token: 0x040019F8 RID: 6648
        ch5s5_SolaceMove,
        // Token: 0x040019F9 RID: 6649
        ch5s5_TWGunsOutVerb_3,
        // Token: 0x040019FA RID: 6650
        ch5s5_TWGunsAway,
        // Token: 0x040019FB RID: 6651
        ch5s5_TWGunsOutVerb,
        // Token: 0x040019FC RID: 6652
        ch5s5_DTThrow2 = 700,
        // Token: 0x040019FD RID: 6653
        ch5s5_OSStomp,
        // Token: 0x040019FE RID: 6654
        Ch3S0_SRFS,
        // Token: 0x040019FF RID: 6655
        Ch3S0_VaultOpen,
        // Token: 0x04001A00 RID: 6656
        Ch3S0_FS1,
        // Token: 0x04001A01 RID: 6657
        Ch3S0_Brainwash,
        // Token: 0x04001A02 RID: 6658
        Ch3S0_VaultAmb,
        // Token: 0x04001A03 RID: 6659
        Ch3S0_HallwayAmb,
        // Token: 0x04001A04 RID: 6660
        IreneGunAimLong,
        // Token: 0x04001A05 RID: 6661
        ch1s8b_HeadDrop,
        // Token: 0x04001A06 RID: 6662
        IreneGunAim,
        // Token: 0x04001A07 RID: 6663
        IreneGrabGun = 713,
        // Token: 0x04001A08 RID: 6664
        GroundedCopter,
        // Token: 0x04001A09 RID: 6665
        ch1s8bWallUp = 716,
        // Token: 0x04001A0A RID: 6666
        ch1s12_DJVTurn,
        // Token: 0x04001A0B RID: 6667
        ch1s12_DJDrivingLoop,
        // Token: 0x04001A0C RID: 6668
        ch1s12_DJVJumpAway,
        // Token: 0x04001A0D RID: 6669
        ch1s12_DJVLand,
        // Token: 0x04001A0E RID: 6670
        ch1s12_DJVLand2 = 722,
        // Token: 0x04001A0F RID: 6671
        Ch1s7_WallImpact,
        // Token: 0x04001A10 RID: 6672
        Ch1s7_TurnAround,
        // Token: 0x04001A11 RID: 6673
        DJVMines_Phase2,
        // Token: 0x04001A12 RID: 6674
        DJV_TransistionJump03 = 728,
        // Token: 0x04001A13 RID: 6675
        DJV_TransistionFall,
        // Token: 0x04001A14 RID: 6676
        RedFall2_Phase2,
        // Token: 0x04001A15 RID: 6677
        ch2s5b_Landing,
        // Token: 0x04001A16 RID: 6678
        ch2s5b_SniperJump,
        // Token: 0x04001A17 RID: 6679
        ch2s5b_CW2,
        // Token: 0x04001A18 RID: 6680
        ch2s5b_HoodBack,
        // Token: 0x04001A19 RID: 6681
        ch2s5b_CW1,
        // Token: 0x04001A1A RID: 6682
        ch5s6b_CasketSlam2 = 737,
        // Token: 0x04001A1B RID: 6683
        ch5s6b_CasketOpen,
        // Token: 0x04001A1C RID: 6684
        ch5s6b_SnakeArmUpOpen,
        // Token: 0x04001A1D RID: 6685
        ch5s6b_SnakeSolaceGrab2,
        // Token: 0x04001A1E RID: 6686
        ch5s6b_SnakeHand_Throw,
        // Token: 0x04001A1F RID: 6687
        ch5s6b_TWFS,
        // Token: 0x04001A20 RID: 6688
        ch5s6b_EndingZoom,
        // Token: 0x04001A21 RID: 6689
        ch5s6b_BodyFall,
        // Token: 0x04001A22 RID: 6690
        ch5s4_Fall,
        // Token: 0x04001A23 RID: 6691
        ch5s6b_HeadSnap3,
        // Token: 0x04001A24 RID: 6692
        ch5s6b_SnakeRumble,
        // Token: 0x04001A25 RID: 6693
        ch5s6b_SnakeLeave,
        // Token: 0x04001A26 RID: 6694
        ch5s6b_SnakeFS1,
        // Token: 0x04001A27 RID: 6695
        ch5s6b_SnakeOpen3,
        // Token: 0x04001A28 RID: 6696
        ch5s6b_TWHSFall,
        // Token: 0x04001A29 RID: 6697
        ch5s6b_SnakeFS2,
        // Token: 0x04001A2A RID: 6698
        ch5s6b_HeadReattach,
        // Token: 0x04001A2B RID: 6699
        ch5s6b_SnakeArrivalFINAL,
        // Token: 0x04001A2C RID: 6700
        ch5s6b_SnakeGrabRed,
        // Token: 0x04001A2D RID: 6701
        ch5s6b_CorpseFall,
        // Token: 0x04001A2E RID: 6702
        ch5s6b_IreneLanding,
        // Token: 0x04001A2F RID: 6703
        ch5s1_DoorKnock,
        // Token: 0x04001A30 RID: 6704
        ch5s1_DoorSmash,
        // Token: 0x04001A31 RID: 6705
        PoliceFSLONG,
        // Token: 0x04001A32 RID: 6706
        Ch5s1_IreneFS,
        // Token: 0x04001A33 RID: 6707
        ch5s1_Amb,
        // Token: 0x04001A34 RID: 6708
        ch5s1_FauxInteractionSFX,
        // Token: 0x04001A35 RID: 6709
        ch5s1_FindCorpse,
        // Token: 0x04001A36 RID: 6710
        ch5s1_EndStinger,
        // Token: 0x04001A37 RID: 6711
        Police_ServerRoom_Ambience,
        // Token: 0x04001A38 RID: 6712
        ch5s4_GraffitiSpray3,
        // Token: 0x04001A39 RID: 6713
        ch5s4_Alley,
        // Token: 0x04001A3A RID: 6714
        ch5s4_FelixLand,
        // Token: 0x04001A3B RID: 6715
        ch5s4_GraffitiSpray2,
        // Token: 0x04001A3C RID: 6716
        PoliceFSLONG_2,
        // Token: 0x04001A3D RID: 6717
        ch5s4_Fan_Loop,
        // Token: 0x04001A3E RID: 6718
        ch1s5b_cm3,
        // Token: 0x04001A3F RID: 6719
        ch1s5b_cm,
        // Token: 0x04001A40 RID: 6720
        ch1s5b_cm2,
        // Token: 0x04001A41 RID: 6721
        pigeon_temp,
        // Token: 0x04001A42 RID: 6722
        ch1s11b_FauxPort,
        // Token: 0x04001A43 RID: 6723
        ch4s3_vinylHeadTurn,
        // Token: 0x04001A44 RID: 6724
        ch4s3_RedStand_2,
        // Token: 0x04001A45 RID: 6725
        ch4s3_camera_2,
        // Token: 0x04001A46 RID: 6726
        ch1s11a_DreamDoor,
        // Token: 0x04001A47 RID: 6727
        ch4s3_camera_1,
        // Token: 0x04001A48 RID: 6728
        ch4s3_RedStand = 784,
        // Token: 0x04001A49 RID: 6729
        ch1s8b_flex,
        // Token: 0x04001A4A RID: 6730
        ch1s11b_DreamEnd,
        // Token: 0x04001A4B RID: 6731
        ch4s3_memorywhoosh,
        // Token: 0x04001A4C RID: 6732
        ch4s3_RedFix,
        // Token: 0x04001A4D RID: 6733
        ch1s11a_GiantDJ,
        // Token: 0x04001A4E RID: 6734
        femaleHey,
        // Token: 0x04001A4F RID: 6735
        femaleWatchIt,
        // Token: 0x04001A50 RID: 6736
        femaleLookOut,
        // Token: 0x04001A51 RID: 6737
        maleBusiness = 794,
        // Token: 0x04001A52 RID: 6738
        bird,
        // Token: 0x04001A53 RID: 6739
        maleScream,
        // Token: 0x04001A54 RID: 6740
        maleGasp,
        // Token: 0x04001A55 RID: 6741
        maleRobo,
        // Token: 0x04001A56 RID: 6742
        Crow,
        // Token: 0x04001A57 RID: 6743
        Seagull,
        // Token: 0x04001A58 RID: 6744
        ch2s6b_EclipseCrewMove,
        // Token: 0x04001A59 RID: 6745
        ch2s6b_EnterDream,
        // Token: 0x04001A5A RID: 6746
        ch2s6b_EclipseSoloMove,
        // Token: 0x04001A5B RID: 6747
        ch2s6b_EclipseFortuneStart,
        // Token: 0x04001A5C RID: 6748
        ch2s6b_EclipseFortuneEnd,
        // Token: 0x04001A5D RID: 6749
        roboOldhead1 = 807,
        // Token: 0x04001A5E RID: 6750
        whatsGoodLow,
        // Token: 0x04001A5F RID: 6751
        roboOldhead2 = 810,
        // Token: 0x04001A60 RID: 6752
        huh3,
        // Token: 0x04001A61 RID: 6753
        notice,
        // Token: 0x04001A62 RID: 6754
        gasp,
        // Token: 0x04001A63 RID: 6755
        hu,
        // Token: 0x04001A64 RID: 6756
        nah,
        // Token: 0x04001A65 RID: 6757
        ch2s7b_Static = 817,
        // Token: 0x04001A66 RID: 6758
        ch2s7b_fall,
        // Token: 0x04001A67 RID: 6759
        VoiceBlockGuyDisagree = 820,
        // Token: 0x04001A68 RID: 6760
        VoiceOldheadRoboTalk,
        // Token: 0x04001A69 RID: 6761
        VoiceOldheadsDialogue,
        // Token: 0x04001A6A RID: 6762
        VoiceBlockGuySurprise,
        // Token: 0x04001A6B RID: 6763
        VoiceSpaceGirlSurprise,
        // Token: 0x04001A6C RID: 6764
        VoiceSpaceGirlNotice,
        // Token: 0x04001A6D RID: 6765
        VoiceOldheadLadyTalk,
        // Token: 0x04001A6E RID: 6766
        MallDreamEnd_Echo1,
        // Token: 0x04001A6F RID: 6767
        MallDreamEnd_Verb,
        // Token: 0x04001A70 RID: 6768
        MallDreamEnd_Echo2,
        // Token: 0x04001A71 RID: 6769
        TaxiPeelOutMission,
        // Token: 0x04001A72 RID: 6770
        TaxiPeelOutMission2,
        // Token: 0x04001A73 RID: 6771
        TaxiStart,
        // Token: 0x04001A74 RID: 6772
        PoliceSoloFS_02 = 834,
        // Token: 0x04001A75 RID: 6773
        PoliceSoloFS_04,
        // Token: 0x04001A76 RID: 6774
        TrapDoorClose,
        // Token: 0x04001A77 RID: 6775
        TrapDoorOpen,
        // Token: 0x04001A78 RID: 6776
        MasterFoleyClip,
        // Token: 0x04001A79 RID: 6777
        ch4s5b_DoorCreak_02 = 840,
        // Token: 0x04001A7A RID: 6778
        ch4s5b_DoorCreak_03,
        // Token: 0x04001A7B RID: 6779
        ch5s6b_BodyFall2,
        // Token: 0x04001A7C RID: 6780
        ch4s5b_DoorCreak_01,
        // Token: 0x04001A7D RID: 6781
        ch4s5b_SolaceImpact,
        // Token: 0x04001A7E RID: 6782
        ch4s5b_SolaceFoley,
        // Token: 0x04001A7F RID: 6783
        LowCameraWhoosh,
        // Token: 0x04001A80 RID: 6784
        cutscene_landing_01,
        // Token: 0x04001A81 RID: 6785
        cutscene_landing_04,
        // Token: 0x04001A82 RID: 6786
        cutscene_landing_03,
        // Token: 0x04001A83 RID: 6787
        cutscene_landing_05,
        // Token: 0x04001A84 RID: 6788
        cutscene_landing_06,
        // Token: 0x04001A85 RID: 6789
        cutscene_landing_02,
        // Token: 0x04001A86 RID: 6790
        NoPackJump_02,
        // Token: 0x04001A87 RID: 6791
        NoPackJump_06,
        // Token: 0x04001A88 RID: 6792
        NoPackJump_01,
        // Token: 0x04001A89 RID: 6793
        NoPackJump_04,
        // Token: 0x04001A8A RID: 6794
        NoPackJump_08,
        // Token: 0x04001A8B RID: 6795
        NoPackJump_03,
        // Token: 0x04001A8C RID: 6796
        NoPackJump_05,
        // Token: 0x04001A8D RID: 6797
        NoPackJump_07,
        // Token: 0x04001A8E RID: 6798
        ch4s6a_DJ_Jump,
        // Token: 0x04001A8F RID: 6799
        ch4s6a_JumpInVehicle,
        // Token: 0x04001A90 RID: 6800
        femaleGasp,
        // Token: 0x04001A91 RID: 6801
        ch4s6a_DJVLand = 865,
        // Token: 0x04001A92 RID: 6802
        ch4s6a_hatchclose,
        // Token: 0x04001A93 RID: 6803
        ch4s7a_DreamEnd,
        // Token: 0x04001A94 RID: 6804
        ch4s7a_Pad,
        // Token: 0x04001A95 RID: 6805
        ch4s7a_IntroWhoosh,
        // Token: 0x04001A96 RID: 6806
        ch4s7a_Ambience = 871,
        // Token: 0x04001A97 RID: 6807
        ch4s7b_punch,
        // Token: 0x04001A98 RID: 6808
        ch4s7b_DJMaskFall_2,
        // Token: 0x04001A99 RID: 6809
        ch4s7b_DJMaskFall,
        // Token: 0x04001A9A RID: 6810
        VoiceBlockGuyDialogue,
        // Token: 0x04001A9B RID: 6811
        SnakeArmImpact_05,
        // Token: 0x04001A9C RID: 6812
        SnakeArmImpact_01,
        // Token: 0x04001A9D RID: 6813
        SnakeChase_SnakeMovement,
        // Token: 0x04001A9E RID: 6814
        SnakeArmImpact_02,
        // Token: 0x04001A9F RID: 6815
        SnakeArmImpact_04,
        // Token: 0x04001AA0 RID: 6816
        SnakeArmImpact_03,
        // Token: 0x04001AA1 RID: 6817
        snakeroar_02,
        // Token: 0x04001AA2 RID: 6818
        VoiceChiefDialogue,
        // Token: 0x04001AA3 RID: 6819
        VoiceSpaceGirlDialogue,
        // Token: 0x04001AA4 RID: 6820
        SnakeBossFinalScene_FelixJump,
        // Token: 0x04001AA5 RID: 6821
        SnakeBossFinalScene_FelixLandWhoosh,
        // Token: 0x04001AA6 RID: 6822
        SnakeBossFinalScene_JetPackImpact_02,
        // Token: 0x04001AA7 RID: 6823
        SnakeBossFinalScene_HeadCrack,
        // Token: 0x04001AA8 RID: 6824
        SnakeBossFinalScene_FelixFlipBoost,
        // Token: 0x04001AA9 RID: 6825
        SnakeBossFinalScene_FelixLandImpact,
        // Token: 0x04001AAA RID: 6826
        SnakeBossFinalScene_HeadImpact,
        // Token: 0x04001AAB RID: 6827
        SnakeBossFinalScene_JetPackPass_01,
        // Token: 0x04001AAC RID: 6828
        SnakeBossFinalScene_ACLoopEnding,
        // Token: 0x04001AAD RID: 6829
        SnakeBossFinalScene_JetPackImpact_01,
        // Token: 0x04001AAE RID: 6830
        SnakeBossFinalScene_Close_02,
        // Token: 0x04001AAF RID: 6831
        SnakeBossFinalScene_Close_01,
        // Token: 0x04001AB0 RID: 6832
        SnakeBossFinalScene_HeadShatter,
        // Token: 0x04001AB1 RID: 6833
        SnakeBossFinalScene_JetPackPass_02,
        // Token: 0x04001AB2 RID: 6834
        SnakeBossFinalScene_SolaceJP,
        // Token: 0x04001AB3 RID: 6835
        SnakeBossFinalScene_SnakeFall,
        // Token: 0x04001AB4 RID: 6836
        SnakeBossFinalScene_FelixBoostIncoming,
        // Token: 0x04001AB5 RID: 6837
        SnakeBossFinalScene_Open,
        // Token: 0x04001AB6 RID: 6838
        ch1s4_DoorCreak,
        // Token: 0x04001AB7 RID: 6839
        ch1s4_Clap_02,
        // Token: 0x04001AB8 RID: 6840
        ch4s2_Blood_2,
        // Token: 0x04001AB9 RID: 6841
        ch4s2_EscherFall,
        // Token: 0x04001ABA RID: 6842
        Ch4s2_Snipershot_03,
        // Token: 0x04001ABB RID: 6843
        ch4s2_VinylHit,
        // Token: 0x04001ABC RID: 6844
        ch4s2_EscherMalfuntion,
        // Token: 0x04001ABD RID: 6845
        ch4s2_Tinnitus,
        // Token: 0x04001ABE RID: 6846
        Ch4s2_SniperArrival_02,
        // Token: 0x04001ABF RID: 6847
        ch4s5b_portapotty_rumble_2 = 913,
        // Token: 0x04001AC0 RID: 6848
        ch4s5b_portapotty_rumble_1,
        // Token: 0x04001AC1 RID: 6849
        ch2s5a_Bullet_1,
        // Token: 0x04001AC2 RID: 6850
        ch2s5a_Bullet_3,
        // Token: 0x04001AC3 RID: 6851
        ch2s5a_Bullet_4,
        // Token: 0x04001AC4 RID: 6852
        ch2s5a_Bullet_2,
        // Token: 0x04001AC5 RID: 6853
        VoiceMedusaDialogue = 921,
        // Token: 0x04001AC6 RID: 6854
        dialogueconfirm,
        // Token: 0x04001AC7 RID: 6855
        Solace_KnockDown_FenceFall,
        // Token: 0x04001AC8 RID: 6856
        Solace_KnockDown_ScreenShake,
        // Token: 0x04001AC9 RID: 6857
        maleChill,
        // Token: 0x04001ACA RID: 6858
        maleMoan,
        // Token: 0x04001ACB RID: 6859
        maleBro,
        // Token: 0x04001ACC RID: 6860
        VoiceEightBallDialogue = 929,
        // Token: 0x04001ACD RID: 6861
        ShieldCopDeath,
        // Token: 0x04001ACE RID: 6862
        ShieldCopEngage,
        // Token: 0x04001ACF RID: 6863
        ShieldCopBlock,
        // Token: 0x04001AD0 RID: 6864
        ShieldCopAttack,
        // Token: 0x04001AD1 RID: 6865
        ShieldCopHurt,
        // Token: 0x04001AD2 RID: 6866
        ShieldCopSpawn,
        // Token: 0x04001AD3 RID: 6867
        BasicCopSpawn,
        // Token: 0x04001AD4 RID: 6868
        SniperDeath,
        // Token: 0x04001AD5 RID: 6869
        SniperEngage,
        // Token: 0x04001AD6 RID: 6870
        SniperHurt,
        // Token: 0x04001AD7 RID: 6871
        ch5s4_screenshake,
        // Token: 0x04001AD8 RID: 6872
        VoiceDJDialogue,
        // Token: 0x04001AD9 RID: 6873
        VoiceFrankDialogue,
        // Token: 0x04001ADA RID: 6874
        Ch3s5_bodyreveal,
        // Token: 0x04001ADB RID: 6875
        ch3s5_slowclap,
        // Token: 0x04001ADC RID: 6876
        Ch3s5_Amb,
        // Token: 0x04001ADD RID: 6877
        ch3s5_FaceReveal,
        // Token: 0x04001ADE RID: 6878
        ch3s5_EnterDream,
        // Token: 0x04001ADF RID: 6879
        VoiceMetalhead,
        // Token: 0x04001AE0 RID: 6880
        VoiceLegend,
        // Token: 0x04001AE1 RID: 6881
        VoiceSniperCaptainDialogue,
        // Token: 0x04001AE2 RID: 6882
        VoiceBlockGuyYellDialogue,
        // Token: 0x04001AE3 RID: 6883
        VoiceOldheadBoomboxTalk,
        // Token: 0x04001AE4 RID: 6884
        VentLoop,
        // Token: 0x04001AE5 RID: 6885
        VentStart,
        // Token: 0x04001AE6 RID: 6886
        VentEnd,
        // Token: 0x04001AE7 RID: 6887
        VoiceTaxiDialogue,
        // Token: 0x04001AE8 RID: 6888
        MineExplosion = 984,
        // Token: 0x04001AE9 RID: 6889
        MineBeep,
        // Token: 0x04001AEA RID: 6890
        VoiceGirl1Dialogue,
        // Token: 0x04001AEB RID: 6891
        FinalBossGunStart,
        // Token: 0x04001AEC RID: 6892
        VoiceDummyDialogue,
        // Token: 0x04001AED RID: 6893
        DJVinylLoop,
        // Token: 0x04001AEE RID: 6894
        VoiceHeadmanDialogue,
        // Token: 0x04001AEF RID: 6895
        VoicePrinceDialogue,
        // Token: 0x04001AF0 RID: 6896
        VoiceJetpackBossDialogue,
        // Token: 0x04001AF1 RID: 6897
        MachineGunStartVentingAnimation,
        // Token: 0x04001AF2 RID: 6898
        MachineGunStopVentingAnimation,
        // Token: 0x04001AF3 RID: 6899
        RailRideStart,
        // Token: 0x04001AF4 RID: 6900
        startMortar,
        // Token: 0x04001AF5 RID: 6901
        stopMortar,
        // Token: 0x04001AF6 RID: 6902
        ch3s5_claps2,
        // Token: 0x04001AF7 RID: 6903
        ch3s5_ScreenShake = 1000,
        // Token: 0x04001AF8 RID: 6904
        MallDoorClose,
        // Token: 0x04001AF9 RID: 6905
        MallDoorOpen,
        // Token: 0x04001AFA RID: 6906
        ShortCircuit,
        // Token: 0x04001AFB RID: 6907
        BombEater_Alarm_Final,
        // Token: 0x04001AFC RID: 6908
        BombEater_GameOver_Final = 1006,
        // Token: 0x04001AFD RID: 6909
        BombEater_Movement_Final,
        // Token: 0x04001AFE RID: 6910
        BombEater_RushModeActivated_Final,
        // Token: 0x04001AFF RID: 6911
        BombEater_CoinSFX_Final,
        // Token: 0x04001B00 RID: 6912
        MAXID_RESERVED,
        // Token: 0x04001B01 RID: 6913
        MAX
    }
}
