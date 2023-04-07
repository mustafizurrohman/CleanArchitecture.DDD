using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAddr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "ID", "City", "Country", "CreatedOn", "StreetAddress", "UpdatedOn", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("00dbe4a4-3951-4699-a46b-8acde08f0a38"), "Jamrozyscheid", "Schweiz", null, "Lohrbergstr.", null, "67147" },
                    { new Guid("0149710f-f951-4f1d-ace2-e3a399fa602e"), "Ost Sammy", "Osterreich", null, "Kinderhausen", null, "88181" },
                    { new Guid("016396c9-9f65-40c3-b7ec-2c457beb6bb0"), "Alt Markdorf", "Deutschland", null, "Grünstr.", null, "30833" },
                    { new Guid("01ad1157-e814-4a3e-967a-175dc6547391"), "Bad Lukasland", "Deutschland", null, "Martin-Luther-Str.", null, "57433" },
                    { new Guid("023464bc-b29d-4a19-b7e1-96c59fa17d30"), "West Carolin", "Osterreich", null, "Schlehdornstr.", null, "62762" },
                    { new Guid("02b5340b-f8a4-4572-8577-f28b209480a9"), "Krauseburg", "Schweiz", null, "Friedlieb-Ferdinand-Runge-Str.", null, "88436" },
                    { new Guid("05e57f0c-d1cb-47ac-b6ad-f7879baa7e37"), "Obermaierscheid", "Schweiz", null, "Am Nonnenbruch", null, "24426" },
                    { new Guid("08eb0b99-4819-4430-8fae-0ee78d653952"), "Zehrascheid", "Schweiz", null, "Linienstr.", null, "51980" },
                    { new Guid("0ce5fba0-0cf0-4eec-87d8-c10c4bc14eb5"), "West Keno", "Osterreich", null, "Tempelhofer Str.", null, "76603" },
                    { new Guid("138a3932-a114-48d9-9225-c1b0f51fba8a"), "Alt Alfredburg", "Schweiz", null, "Strombergstr.", null, "88290" },
                    { new Guid("189aa5f2-fa09-417e-86cd-ba39a2ae5fb0"), "Neu Mariam", "Schweiz", null, "Dönhoffstr.", null, "50251" },
                    { new Guid("19db99b7-50cd-4d29-bc9d-05c1bf240a59"), "Bad Nielsland", "Schweiz", null, "Feuerdornweg", null, "19394" },
                    { new Guid("1cac9171-f001-43ff-bed5-e07c5220aa37"), "Michallekdorf", "Deutschland", null, "Kyllstr.", null, "76209" },
                    { new Guid("1cde8477-9470-4efa-b36d-54b24ebe9a78"), "Neu Hailey", "Deutschland", null, "Bergstr.", null, "13775" },
                    { new Guid("1d33fcc5-427d-4a46-b4af-d2da015e96fc"), "Mullerstadt", "Osterreich", null, "Carl-Rumpff-Str.", null, "98311" },
                    { new Guid("1e96ec6a-5941-4397-989f-c37dabb574fd"), "Süd Benjaminscheid", "Schweiz", null, "Unter dem Schildchen", null, "32205" },
                    { new Guid("227a4782-3d98-4d58-a94e-b3b0d76f5f84"), "Floredorf", "Osterreich", null, "Am Plattenbusch", null, "78155" },
                    { new Guid("22906b97-a4cb-4824-9058-aaa22d182ff7"), "Madetzkyland", "Schweiz", null, "Taubenweg", null, "65752" },
                    { new Guid("2672623a-26ac-41ae-8ff4-11a1d553d924"), "Süd Christianstadt", "Schweiz", null, "Feuerbachstr.", null, "49666" },
                    { new Guid("28354db4-2dfe-433c-ac52-e4b3a6ced106"), "Süd Thiesstadt", "Osterreich", null, "Ruhlachstr.", null, "11097" },
                    { new Guid("286124e3-efd3-4ffb-b636-fdce91c08ae7"), "Salowstadt", "Deutschland", null, "Wilhelm-Busch-Str.", null, "59018" },
                    { new Guid("293b80b5-b6a7-41ae-b9f4-0e5c76b2de91"), "Kimscheid", "Deutschland", null, "Auf der Ohmer", null, "59891" },
                    { new Guid("2a7a5d7c-7c81-4c78-bddf-ba9f085a50b4"), "Neu Aliyadorf", "Deutschland", null, "Am Weingarten", null, "09192" },
                    { new Guid("2d9849e2-5b43-4a84-9163-3be159e1ef95"), "Bad Jellaland", "Osterreich", null, "Niederfeldstr.", null, "75149" },
                    { new Guid("35ae2c93-0f3d-4857-b22e-dd1efca4fc0d"), "Florascheid", "Schweiz", null, "Ginsterweg", null, "69443" },
                    { new Guid("3824358b-e3a6-43d4-9e16-a4a138027bcc"), "Erwesstadt", "Osterreich", null, "Quarzstr.", null, "06845" },
                    { new Guid("397f954f-af81-4b53-af18-997cc56eb08e"), "Ost Merveburg", "Osterreich", null, "Hitdorfer Kirchweg", null, "59670" },
                    { new Guid("3a27397d-7f88-48f2-8596-204112f44b9b"), "Neu Jesperburg", "Schweiz", null, "Freudenthaler Weg", null, "49848" },
                    { new Guid("3ac75e17-5a9b-44e5-99b9-39667cceaefc"), "Weimerland", "Deutschland", null, "Nauener Str.", null, "80624" },
                    { new Guid("3e77dd42-76bf-46e6-a480-da84b523efd2"), "Evaburg", "Deutschland", null, "Friedhofstr.", null, "17655" },
                    { new Guid("40fa84d2-b15e-4115-a9ef-917934b48a49"), "Alt Thore", "Deutschland", null, "Heimstättenweg", null, "11439" },
                    { new Guid("46eece4d-7027-48ca-9ca2-7255ea8005b6"), "Schottstadt", "Deutschland", null, "Bürriger Weg", null, "22375" },
                    { new Guid("4959d14d-fe40-471d-90d2-d61edb166793"), "Süd Summer", "Deutschland", null, "Auf der Ohmer", null, "23067" },
                    { new Guid("4b534b8b-1ac6-430e-9aa6-70fb4937be8f"), "Neu Leanndorf", "Osterreich", null, "Röttgerweg", null, "66402" },
                    { new Guid("4f19e7f0-01df-4708-8f5f-34b68e6fa4e4"), "Mortendorf", "Osterreich", null, "Willy-Brandt-Ring", null, "95355" },
                    { new Guid("4f38a541-78be-4692-afe2-2aa79b038d87"), "Nord Caitlindorf", "Osterreich", null, "An den Irlen", null, "83572" },
                    { new Guid("501d72f7-e291-4ab1-993b-e2b4b4fdfffd"), "Alt Julianeburg", "Schweiz", null, "Wilhelmsgasse", null, "94769" },
                    { new Guid("50caad02-b9aa-45ed-b61d-7533a8b1bfd2"), "Böhmdorf", "Osterreich", null, "Grundermühle", null, "53260" },
                    { new Guid("5288594f-f6a1-40f1-9536-7acaa976f86a"), "Jacobstadt", "Osterreich", null, "Am Köllerweg", null, "60572" },
                    { new Guid("5333e837-63d1-4e61-a13f-dd90b943863e"), "Süd Isabel", "Osterreich", null, "Pfeilshofstr.", null, "81522" },
                    { new Guid("550240a3-b5f9-470c-abdb-00e66a38d560"), "Neu Leon", "Osterreich", null, "Opladener Platz", null, "87973" },
                    { new Guid("59da2cee-5036-4dc9-a337-2b2f62f7d7fb"), "Paulineland", "Deutschland", null, "Neuenkamp", null, "12093" },
                    { new Guid("5de7963b-5a45-4c59-b0d6-1376e2f11f25"), "Jorisburg", "Schweiz", null, "Carl-Leverkus-Str.", null, "14683" },
                    { new Guid("5eb1db1e-ed38-4cc4-b7b7-6f823171e173"), "Neu Riccardo", "Osterreich", null, "Am Thelenhof", null, "45206" },
                    { new Guid("6302e69c-399a-45c0-a4fe-522b629223e2"), "Deanburg", "Schweiz", null, "Grüner Weg", null, "75903" },
                    { new Guid("6536258b-294c-4101-b39a-f9f438ba916b"), "Ost Ashley", "Osterreich", null, "Am Scherfenbrand", null, "13238" },
                    { new Guid("661b25cb-d993-43c0-aea3-ad402ad8cdc4"), "Ost Oscarland", "Schweiz", null, "Am Hühnerberg", null, "19819" },
                    { new Guid("6781a41d-8ef4-4bc8-b140-fbe3a564e07b"), "Nord Linostadt", "Schweiz", null, "Ottweilerstr.", null, "61712" },
                    { new Guid("6e4086e9-63bb-4248-ac83-a327d3c0b1de"), "Ost Marlene", "Deutschland", null, "Wiembachallee", null, "50047" },
                    { new Guid("6ec61cd7-8bfe-447a-a06e-9aaaacabdd67"), "Neu Emmydorf", "Osterreich", null, "Kantstr.", null, "28053" },
                    { new Guid("73d15da9-b54a-484a-b840-28260662f564"), "Bad Yarascheid", "Schweiz", null, "Katzbachstr.", null, "75876" },
                    { new Guid("7693b453-7f93-4632-bc76-131c784e605a"), "Ost Linnea", "Osterreich", null, "Weiherfeld", null, "10616" },
                    { new Guid("7a3baaf2-97f4-497e-9b49-ea3301e7ca7a"), "Chantaldorf", "Deutschland", null, "St. Ingberter Str.", null, "65339" },
                    { new Guid("7dacb2f6-6ce7-49e9-b06a-c78a24d21abf"), "Bad Lydiascheid", "Osterreich", null, "Halfenleimbach", null, "44186" },
                    { new Guid("80d0f792-4bbf-4c2e-bca2-7ced4b95f5b9"), "Nord Allegra", "Osterreich", null, "Sonderburger Str.", null, "34238" },
                    { new Guid("85761bcb-c169-43b1-9d92-675d585821d2"), "Nord Jasmine", "Osterreich", null, "Küppersteger Str.", null, "55149" },
                    { new Guid("8a9ede42-2e69-4c19-8e2b-1044b0ed9850"), "Bad Rianaburg", "Deutschland", null, "Eintrachtstr.", null, "10151" },
                    { new Guid("8b77328a-7700-41f8-b4f3-1bdcc6650ac0"), "Lindaburg", "Osterreich", null, "Driescher Hecke", null, "76633" },
                    { new Guid("8cd6c7f0-5d63-46e4-b5de-1a73045a37a6"), "Neu Mayaland", "Deutschland", null, "Sperberweg", null, "31764" },
                    { new Guid("918be368-428f-4d1a-ab0f-afa9277deb31"), "Wujakland", "Osterreich", null, "Am Stadtpark", null, "76876" },
                    { new Guid("966ec860-4bed-4e92-881b-a77012808e71"), "Jannekscheid", "Schweiz", null, "Merziger Str.", null, "88835" },
                    { new Guid("97584810-e6ca-4fa6-9cb2-b250c9afe466"), "Bernerburg", "Deutschland", null, "Mühlenweg", null, "68739" },
                    { new Guid("9aa14864-8383-4e04-90c9-9c3a34ea3c1d"), "Neu Gerrit", "Osterreich", null, "Auf der Grieße", null, "72167" },
                    { new Guid("9b567c54-d378-4a58-b5e7-af76c507da58"), "Bad Tamialand", "Deutschland", null, "Im Eisholz", null, "75992" },
                    { new Guid("9dd65635-015a-4a57-ad4a-85e6d36de9e9"), "Torbenstadt", "Osterreich", null, "Marie-Curie-Str.", null, "58601" },
                    { new Guid("a010d7fd-cda3-474f-9dde-421465280fa1"), "Süd Maxim", "Deutschland", null, "Im Steinfeld", null, "72206" },
                    { new Guid("a35512de-142d-40c9-b849-d7df895dd01e"), "Waschbüschdorf", "Schweiz", null, "Saarstr.", null, "71184" },
                    { new Guid("aacd9c10-901f-4f30-992d-8673c51f4518"), "Nord Kimberlyland", "Deutschland", null, "Rheinallee", null, "96659" },
                    { new Guid("b3bfa100-c430-4626-b412-5c191c86a616"), "Bad Jano", "Deutschland", null, "Albert-Zarthe-Weg", null, "22604" },
                    { new Guid("b414749b-87bf-4b57-8511-bacb35e53bc1"), "Nord Jannestadt", "Schweiz", null, "Raushofstr.", null, "04314" },
                    { new Guid("b434a206-d5b0-46bd-b8d1-caab32c54e7c"), "Majaburg", "Schweiz", null, "Oskar-Moll-Str.", null, "16740" },
                    { new Guid("b70ef8e1-0470-4fc4-a979-03a5b8d51dea"), "Alt Julika", "Schweiz", null, "Lerchengasse", null, "71570" },
                    { new Guid("b72d5c3c-8b3f-4a45-91b6-e4a557c644bd"), "Maximastadt", "Schweiz", null, "Samlandstr.", null, "93946" },
                    { new Guid("b77d3a11-be7d-4f1b-a8ed-28df83185680"), "Nord Lenny", "Osterreich", null, "Lötzener Str.", null, "06522" },
                    { new Guid("be50bc48-f496-4c6e-81e2-9508e901f6cd"), "West Frankaland", "Schweiz", null, "Humboldtstr.", null, "88585" },
                    { new Guid("be79b73e-ce76-4b67-a5db-60e956bf406a"), "Melisstadt", "Deutschland", null, "Schwarzastr.", null, "92959" },
                    { new Guid("c06a3fda-256d-4a76-a617-63d2a3e8dca2"), "Kruschinskiburg", "Osterreich", null, "Bonifatiusstr.", null, "73278" },
                    { new Guid("c0ef290a-ec8d-4757-ac1b-5841fdc93ed8"), "Ost Christian", "Deutschland", null, "Steinbücheler Feld", null, "29349" },
                    { new Guid("c4479ffd-d261-42e6-9edc-34cc175966f2"), "Alt Laurensburg", "Schweiz", null, "Alte Landstr.", null, "82551" },
                    { new Guid("c5a53d4d-f80e-41b8-84e2-06054326ce85"), "Schönfeldstadt", "Deutschland", null, "Quettinger Str.", null, "64319" },
                    { new Guid("ce5c3fee-db17-4690-96fe-bdd8f455d139"), "West Raphaelburg", "Schweiz", null, "Pescher Busch", null, "79391" },
                    { new Guid("d1d20406-4d81-4f18-84a6-d8f87e4f21ee"), "Nord Martin", "Osterreich", null, "Hornpottweg", null, "80752" },
                    { new Guid("d3ce43b2-06f6-474e-8c99-5417bef150ac"), "West Catrinburg", "Schweiz", null, "Habichtgasse", null, "05549" },
                    { new Guid("d4161cba-9f34-4de4-b6a3-393a8f98a0c4"), "Seilerdorf", "Osterreich", null, "Im Kalkfeld", null, "80073" },
                    { new Guid("d5cd5801-5e60-4659-9284-e0ffc8469ea7"), "Nord Selina", "Deutschland", null, "Sperberweg", null, "11056" },
                    { new Guid("d5f09faf-fdbb-446a-9620-540613d0dfbf"), "Neu Melekdorf", "Deutschland", null, "Bebelstr.", null, "84775" },
                    { new Guid("d7e38809-08c3-4833-a13b-c4ac58a2ad73"), "Spiegelburgland", "Schweiz", null, "Gellertstr.", null, "94960" },
                    { new Guid("d911cb0d-9a3e-4295-a3e1-2c75c100384b"), "Alt Janinaburg", "Deutschland", null, "Holzer Wiesen", null, "74289" },
                    { new Guid("e09337cc-f58b-440e-b226-769155b96111"), "Neu Keremstadt", "Osterreich", null, "Karl-Marx-Str.", null, "37074" },
                    { new Guid("ebfb43b2-3a4a-4e4b-aa09-2ef2214a2323"), "Kaidorf", "Deutschland", null, "Ulrich-von-Hassell-Str.", null, "70577" },
                    { new Guid("edfcff51-b74a-4304-82dd-463da8e029bc"), "Janderscheid", "Osterreich", null, "Ewald-Röll-Str.", null, "66074" },
                    { new Guid("efbff0a9-6af6-4704-bb6e-5f55b9cbc384"), "Hessekdorf", "Osterreich", null, "Im Rosengarten", null, "23454" },
                    { new Guid("f0464e55-ba30-40ee-b8d1-3bac4217ec6e"), "Neu Milo", "Deutschland", null, "Am Sportplatz", null, "78854" },
                    { new Guid("f42d2a38-c93a-4565-8549-fe2f04193f38"), "Hentschelscheid", "Schweiz", null, "Halfenleimbach", null, "49864" },
                    { new Guid("f669afea-dcd1-4f23-a728-e31bd508c1e4"), "Kreissigstadt", "Osterreich", null, "Burscheider Str.", null, "91824" },
                    { new Guid("f77fa7ec-e510-41f2-acf2-9d9b8733d7c0"), "Süd Milaland", "Schweiz", null, "Am Knechtsgraben", null, "53932" },
                    { new Guid("f7838350-c389-493f-8b74-632f20b39b62"), "Albertscheid", "Deutschland", null, "Am Kühnsbusch", null, "18163" },
                    { new Guid("f836be9b-a9bc-4331-a101-af5daace4acd"), "Juliestadt", "Schweiz", null, "Rotdornweg", null, "58731" },
                    { new Guid("f85a3a34-9322-4ba2-962c-efb6219fc138"), "Neu Toni", "Deutschland", null, "Am Heidkamp", null, "36273" },
                    { new Guid("fbb8bc2f-d65d-4d00-a4d7-4dea279882e0"), "Buderburg", "Osterreich", null, "Bruchhauser Str.", null, "10246" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("00dbe4a4-3951-4699-a46b-8acde08f0a38"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("0149710f-f951-4f1d-ace2-e3a399fa602e"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("016396c9-9f65-40c3-b7ec-2c457beb6bb0"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("01ad1157-e814-4a3e-967a-175dc6547391"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("023464bc-b29d-4a19-b7e1-96c59fa17d30"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("02b5340b-f8a4-4572-8577-f28b209480a9"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("05e57f0c-d1cb-47ac-b6ad-f7879baa7e37"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("08eb0b99-4819-4430-8fae-0ee78d653952"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("0ce5fba0-0cf0-4eec-87d8-c10c4bc14eb5"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("138a3932-a114-48d9-9225-c1b0f51fba8a"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("189aa5f2-fa09-417e-86cd-ba39a2ae5fb0"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("19db99b7-50cd-4d29-bc9d-05c1bf240a59"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("1cac9171-f001-43ff-bed5-e07c5220aa37"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("1cde8477-9470-4efa-b36d-54b24ebe9a78"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("1d33fcc5-427d-4a46-b4af-d2da015e96fc"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("1e96ec6a-5941-4397-989f-c37dabb574fd"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("227a4782-3d98-4d58-a94e-b3b0d76f5f84"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("22906b97-a4cb-4824-9058-aaa22d182ff7"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("2672623a-26ac-41ae-8ff4-11a1d553d924"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("28354db4-2dfe-433c-ac52-e4b3a6ced106"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("286124e3-efd3-4ffb-b636-fdce91c08ae7"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("293b80b5-b6a7-41ae-b9f4-0e5c76b2de91"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("2a7a5d7c-7c81-4c78-bddf-ba9f085a50b4"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("2d9849e2-5b43-4a84-9163-3be159e1ef95"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("35ae2c93-0f3d-4857-b22e-dd1efca4fc0d"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("3824358b-e3a6-43d4-9e16-a4a138027bcc"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("397f954f-af81-4b53-af18-997cc56eb08e"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("3a27397d-7f88-48f2-8596-204112f44b9b"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("3ac75e17-5a9b-44e5-99b9-39667cceaefc"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("3e77dd42-76bf-46e6-a480-da84b523efd2"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("40fa84d2-b15e-4115-a9ef-917934b48a49"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("46eece4d-7027-48ca-9ca2-7255ea8005b6"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("4959d14d-fe40-471d-90d2-d61edb166793"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("4b534b8b-1ac6-430e-9aa6-70fb4937be8f"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("4f19e7f0-01df-4708-8f5f-34b68e6fa4e4"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("4f38a541-78be-4692-afe2-2aa79b038d87"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("501d72f7-e291-4ab1-993b-e2b4b4fdfffd"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("50caad02-b9aa-45ed-b61d-7533a8b1bfd2"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("5288594f-f6a1-40f1-9536-7acaa976f86a"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("5333e837-63d1-4e61-a13f-dd90b943863e"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("550240a3-b5f9-470c-abdb-00e66a38d560"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("59da2cee-5036-4dc9-a337-2b2f62f7d7fb"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("5de7963b-5a45-4c59-b0d6-1376e2f11f25"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("5eb1db1e-ed38-4cc4-b7b7-6f823171e173"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("6302e69c-399a-45c0-a4fe-522b629223e2"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("6536258b-294c-4101-b39a-f9f438ba916b"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("661b25cb-d993-43c0-aea3-ad402ad8cdc4"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("6781a41d-8ef4-4bc8-b140-fbe3a564e07b"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("6e4086e9-63bb-4248-ac83-a327d3c0b1de"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("6ec61cd7-8bfe-447a-a06e-9aaaacabdd67"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("73d15da9-b54a-484a-b840-28260662f564"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("7693b453-7f93-4632-bc76-131c784e605a"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("7a3baaf2-97f4-497e-9b49-ea3301e7ca7a"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("7dacb2f6-6ce7-49e9-b06a-c78a24d21abf"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("80d0f792-4bbf-4c2e-bca2-7ced4b95f5b9"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("85761bcb-c169-43b1-9d92-675d585821d2"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("8a9ede42-2e69-4c19-8e2b-1044b0ed9850"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("8b77328a-7700-41f8-b4f3-1bdcc6650ac0"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("8cd6c7f0-5d63-46e4-b5de-1a73045a37a6"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("918be368-428f-4d1a-ab0f-afa9277deb31"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("966ec860-4bed-4e92-881b-a77012808e71"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("97584810-e6ca-4fa6-9cb2-b250c9afe466"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("9aa14864-8383-4e04-90c9-9c3a34ea3c1d"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("9b567c54-d378-4a58-b5e7-af76c507da58"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("9dd65635-015a-4a57-ad4a-85e6d36de9e9"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("a010d7fd-cda3-474f-9dde-421465280fa1"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("a35512de-142d-40c9-b849-d7df895dd01e"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("aacd9c10-901f-4f30-992d-8673c51f4518"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("b3bfa100-c430-4626-b412-5c191c86a616"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("b414749b-87bf-4b57-8511-bacb35e53bc1"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("b434a206-d5b0-46bd-b8d1-caab32c54e7c"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("b70ef8e1-0470-4fc4-a979-03a5b8d51dea"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("b72d5c3c-8b3f-4a45-91b6-e4a557c644bd"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("b77d3a11-be7d-4f1b-a8ed-28df83185680"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("be50bc48-f496-4c6e-81e2-9508e901f6cd"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("be79b73e-ce76-4b67-a5db-60e956bf406a"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("c06a3fda-256d-4a76-a617-63d2a3e8dca2"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("c0ef290a-ec8d-4757-ac1b-5841fdc93ed8"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("c4479ffd-d261-42e6-9edc-34cc175966f2"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("c5a53d4d-f80e-41b8-84e2-06054326ce85"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("ce5c3fee-db17-4690-96fe-bdd8f455d139"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d1d20406-4d81-4f18-84a6-d8f87e4f21ee"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d3ce43b2-06f6-474e-8c99-5417bef150ac"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d4161cba-9f34-4de4-b6a3-393a8f98a0c4"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d5cd5801-5e60-4659-9284-e0ffc8469ea7"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d5f09faf-fdbb-446a-9620-540613d0dfbf"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d7e38809-08c3-4833-a13b-c4ac58a2ad73"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("d911cb0d-9a3e-4295-a3e1-2c75c100384b"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("e09337cc-f58b-440e-b226-769155b96111"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("ebfb43b2-3a4a-4e4b-aa09-2ef2214a2323"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("edfcff51-b74a-4304-82dd-463da8e029bc"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("efbff0a9-6af6-4704-bb6e-5f55b9cbc384"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f0464e55-ba30-40ee-b8d1-3bac4217ec6e"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f42d2a38-c93a-4565-8549-fe2f04193f38"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f669afea-dcd1-4f23-a728-e31bd508c1e4"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f77fa7ec-e510-41f2-acf2-9d9b8733d7c0"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f7838350-c389-493f-8b74-632f20b39b62"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f836be9b-a9bc-4331-a101-af5daace4acd"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("f85a3a34-9322-4ba2-962c-efb6219fc138"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "ID",
                keyValue: new Guid("fbb8bc2f-d65d-4d00-a4d7-4dea279882e0"));
        }
    }
}
