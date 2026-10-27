# TripsTrapsTrull (.NET MAUI rakendus)

See on .NET MAUI raamistikul välja töötatud MVVM OOP põhimõtteid järgiv mäng, mis on mõeldud 
kahele mängijale. Mängijate eesmärgiks on esimesena saada oma sümbolitest horisontaalne, 
vertikaalne või diagonaalne rida. 

Mängu põhiomadused:
    1. Mitme platvormi tugi - mäng töötab nii windowsis kui ka android seadmetes.
    2. Mänguandmete püsivud(SQLite) - mängustatistika salvestamiseks on rakendusele 
        lisatud kohaliku seadme SQLite andmebaas. Andmebaasi salvestatakse mängijate 
        mängitud mängud, võidud, kaotused ja viigid, mida kuvatakse mängustatistika vaates.
        Andmebaasifail (trull_database.db3) salvestatakse rakenduse privaatsesse kausta 
        (FileSystem.AppDataDirectory). Andmebaasi sisu kontrollimiseks ja esitlemiseks 
        saab kasutada tasuta tööriista DB Browser for SQLite.
    3. Mäng Botiga - mängul on olemas võimalus mängida tehisintellekti vastu.
    4. Kohandatav mänguväli - mänguvälja suurust saab valida kolme võimaluse 
        vahel: 3x3, 4x4 või 5x5 ruutu.
    5. Teemade vahetus - heledat ja tumedat teemat saab vahetada reaalajas,
        vaikimisi teemaks on hele teema.
    6. Erinevad sümbolite komplektid (X/O, ♥/○, ★/●) ja värvivalikud - mängija saab
        valida endale sobivad sümbolite ja nende värvide kombinatsioonid.
    
Kasutatud tehnoloogiad:
    Visual Studio (programmeerimisplatvorm)
    C# ja .NET MAUI (multiplatvorm App UI)
    SQLite-net-pcl (kohaliku seadme SQLite andmebaasi haldamiseks)
    SQLite-PCLRaw.bundle_green (SQLite draiverid)
    GitHub (mängu lähtekoodi säilitamine ja versioonihaldus)

Mängu käivitamine:
    1. Veendu, et sul on installeeritud Visual Studio koos .NET MAUI võimalusega 
    (.NET Multi-platform App UI development).
    2. Klooni või laadi alla käesolev repo.
    3. Ava fail (.sln) Visual Studios.
    4. Vali mängu käivitamiseks Windows Machine või Android Emulator.
    5. Vajuta F5 või Start, et projekt kompileerida ja käivitada.


