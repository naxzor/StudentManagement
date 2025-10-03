Dette project demonstrerer change based migrations med entity framework og postgreSQL

Hver ændring i domænemodellen blev fulgt op med en migration, samt kontrol af migration filen, for at sikre vi ville opnå de ønskede ændringer, 
inden der blev lavet en opdatering af databasen. Hvorefter vi kontrollerede igen i databasen at vi fik ænskede resultat

Proces: lavede en consol app, for begge projecter for at holde dem adskilt og sikrde mig git.ignore samt nuget's var installerert.
Herefter installerede jeg postgres samt pgadmin i docker ved at lave docker-compose.yml hvorefter jeg lavede et SQL script til at oprette de nødvendige databaser for begge projecter

Oprettelse af databasen sker automatisk ved hjælp af det script der ligger i db/init, efter kommandoen docker-compose up

Efter jeg var færdig med en initial setup lavede jeg de første models, og lavede en ny branch til dem og lavede en DB context hvor jeg kan styrer enteties.
Jeg opsatte en appsetting.json til at holde min connectionstring for sikkerhed.

trin og begrundelser.

1: Initial schema

Entities Student, Course, enrollment blev oprettet.

PK på id, lavede flere felter til nonnullable så vi sikre data der er vigtigt for de enkelte models kommer med. 
Samt uniq på index for f.eks email for at ungår dubletter

FK'er Student - Enrollment og Course - Enrollment med Cascade sletning (Enrollment er afhøngig data)

2: Student.MiddleName

Her er der valgt String som nullable så folk der ikke har et stadig kan oprettes, samtidig er den non-breaking
for existerende rækker da migration ville blive nægtet grundet der existere koloner uden.

3: Student.DateOfBirth

Her bruger jeg date for at sikre EF ikke laver dem til date time vi ønsker kun date når vi skal lave en fødselsdato.

4: Instructor

Oprettede den nye tabel: Id, FirstName, LastName, Email, HireDate.
Email blev igen sat til uniq så vi ikke får dubletter og lavede FirstName, LastName, Email til nonnullable igen. 
HireDate ved jeg ikke hvor vigtig er så den lavede jeg nullable. yderligere udvide jeg Course-tabellen med et InstructorID 
som blev sat til FK til ID fra instructor

5: Course - Instructor(FK)

eftersom vi nu har en relation mellem de 2 valgte jeg at lave OnDDelete(SetNull) dvs et course kan exsitere uden instructor
. og sletningen nulstiller referencer frem for at slette course.

6: Rename Grade

Her renamede jeg bare til FinalGrade og lavede en migration hvorefter jeg sikrede vi havde nondestructive migration ved at sikre
at EF lavede RenameColumn frem for Drop og så add ny derved beholder vi data.

7: Department

Oprettede ny model/table igen. Satte bugdet her til 12.2 da jeg tænker vi har brug for en højere precision da det penge og 
vi vil ikke have mere end 2 decimaler. yderligere lavede jeg one to one uniq constraint på DepartmentHeadId så en instructor 
kun kan være head for en department. Samtidig er DepartmentHeadId en FK til instructor id med Delete(Restrict) dvs vi kan ikke
slette en instructor hvis den er sat som Head.
Til sidst er FK sat til Course - Deparment så de får et departmentId. Dette betyder at et Course skal tilhøre en Deparment
Samtidig satte jeg Delete(restrict) igen så man ikke kan slette en department hvis den har Course

8: Course.Credit

Her ændrede jeg int til decimel som forskrevet i opgaven.

Overvejelser

igennem hele opgaven har jeg forsøgt at lave non-destructive hvor det muligt. (så man i den virkelig verden) bibeholder data og ungår at skulle lave midlertidig tables/columns
for at migrate data til det nye.
Yderligere har jeg prøvet at opsætte databasen så den afspejler virkeligheden f.eks Middelname som nullable vs notnullable.
Samtidig har jeg prøvet at lave en delete behavior der afspejler en beskyttelse af forretningdata (Restrict på master-tabeller, Cascade på afhængigheder)
og Decimal-præcision er valgt ud fra domæne (bugdet/credit)