Dette Afsnit demonstrerer change based migrations med entity framework og postgreSQL

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



Dette Afsnit demonstrerer State based migrations med Atlas og postgreSQL

først opsatte jeg en directory med 2 filer der hedder state-basedatlas.hcl og basedschema.hcl da vi bruger atlas via container til at vise os Diff på State schema som lave vores SQL.
state-basedatlas.hcl indeholder vores connection til databasen men da det kører i docker henviser vi til en anden container. samtidig er der en DEv url den er kun til at atlas forstår jeg, 
bruger postgreSQL så den ved hvilken databse dialekt den skal bruge

1: Initial schema

ligesom i EF delen har jeg lavet Entities Student, Course, enrollment. disse er skrevett ind i baseschema.hcl hvorfter jeg har kørt kommandoen fra Atlastcommands.txt til at teste diff og derefter
har jeg brugt den anden til at apply det til databsen hvis diff var efter hensigten.
Jeg har brugt samme fremgangsmåde i henhold til constraints og delete så den del vil jeg ikke gå ind i her.
jeg har valgt for at gemme de gamle schema's for bedre oversigt inden opdatering dette er ikke normal men er valgt så man kan se hvordan de så ud de bliver flyttet til en mappe jeg har kaldt migrations
dette afspejler hvordan det er gjort med EF. Herefter opdatere jeg basedschema.hcl (det schema som er sådan databasen skulle se ud)

2: Student.MiddleName

her smider jeg en column med middlename ind sætter den nulable og køre diff og derfter apply

3: Student.DateOfBirth

igen her gør jeg det samme bare med DateOfBirth og sikre den er sat til date kører diff og apply 

4: Instructor

jeg opretter på samme måde en ny tabel schema på Instructor kører diff og apply

5: Course - Instructor(FK)

jeg laver nu en FK mellem Course og instructors i schema'et og laver diff og apply

6: Department

Jeg oprettede en tabel her med department og lavede FK til Course ligesom jeg gjorde i Change migrations og kontrollerede det var som det skulle være med Diff og lavede en apply

8: Course.Credit

til slut skiftede jeg int igen til decimal og kontrollerede det hele var som skulle være med diff og apply


Reflection


jeg synes helt klart at Change er nemmere at arbejde med under udviklingen af et system. men dog har man meget større overblik under State based men det kan være svært at holde tungen lige i munden når projectet bliver stort dog krøver det ingen kode base for at lave database ændringer som med EF der skal man rette i koden for at ændre her har du bare et skema som man applyer på databasen med ændringer og du har større overblik