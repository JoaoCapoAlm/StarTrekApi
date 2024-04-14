using System.Drawing;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class DadosIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var commandSql = new StringBuilder();

            #region Country
            commandSql.Append("INSERT INTO [dbo].[Country] ([ResourceName]) VALUES");
			commandSql.Append("('Afghanistan'), ('Albania'), ('Algeria'), ('AmericanSamoa'), ('Andorra'), ('Angola'), ('Anguilla'),");
			commandSql.Append("('Antarctica'), ('AntiguaAndBarbuda'), ('Argentina'), ('Armenia'), ('Aruba'), ('Australia'),");
			commandSql.Append("('Austria'), ('Azerbaijan'), ('Bahamas'), ('Bahrain'), ('Bangladesh'), ('Barbados'), ('Belarus'),");
			commandSql.Append("('Belgium'), ('Belize'), ('Benin'), ('Bermuda'), ('Bhutan'), ('BosniaAndHerzegovina'), ('Botswana'),");
			commandSql.Append("('BouvetIsland'), ('Brazil'), ('BritishIndianOceanTerritory'), ('BruneiDarussalam'), ('Bulgaria'),");
			commandSql.Append("('BurkinaFaso'), ('Burundi'), ('Cambodia'), ('Cameroon'), ('Canada'), ('CapeVerde'),");
			commandSql.Append("('CaymanIslands'), ('CentralAfricanRepublic'), ('Chad'), ('Chile'), ('China'), ('ChristmasIsland'),");
			commandSql.Append("('CocosKeelingIslands'), ('Colombia'), ('Comoros'), ('Congo'), ('CookIslands'), ('CostaRica'),");
			commandSql.Append("('Croatia'), ('Cuba'), ('Cyprus'), ('CzechRepublic'), ('Denmark'), ('Djibouti'), ('Dominica'),");
			commandSql.Append("('DominicanRepublic'), ('Ecuador'), ('Egypt'), ('ElSalvador'), ('EquatorialGuinea'), ('Eritrea'),");
			commandSql.Append("('Estonia'), ('Ethiopia'), ('FalklandIslandsMalvinas'), ('FaroeIslands'), ('Fiji'), ('Finland'),");
			commandSql.Append("('France'), ('FrenchGuiana'), ('FrenchPolynesia'), ('FrenchSouthernTerritories'), ('Gabon'),");
			commandSql.Append("('Gambia'), ('Georgia'), ('Germany'), ('Ghana'), ('Gibraltar'), ('Greece'), ('Greenland'),");
			commandSql.Append("('Grenada'), ('Guadeloupe'), ('Guam'), ('Guatemala'), ('Guernsey'), ('Guinea'), ('GuineaBissau'),");
			commandSql.Append("('Guyana'), ('Haiti'), ('HeardIslandAndMcDonaldIslands'), ('HolySeeVaticanCityState'), ('Honduras'),");
			commandSql.Append("('HongKong'), ('Hungary'), ('Iceland'), ('India'), ('Indonesia'), ('Iran'), ('Iraq'), ('Ireland'),");
			commandSql.Append("('IsleOfMan'), ('Israel'), ('Italy'), ('Jamaica'), ('Japan'), ('Jersey'), ('Jordan'), ('Kazakhstan'),");
			commandSql.Append("('Kenya'), ('Kiribati'), ('Kuwait'), ('Kyrgyzstan'), ('LaoPeoplesDemocraticRepublic'), ('Latvia'),");
			commandSql.Append("('Lebanon'), ('Lesotho'), ('Liberia'), ('Libya'), ('Liechtenstein'), ('Lithuania'), ('Luxembourg'),");
			commandSql.Append("('Macao'), ('Madagascar'), ('Malawi'), ('Malaysia'), ('Maldives'), ('Mali'), ('Malta'),");
			commandSql.Append("('MarshallIslands'), ('Martinique'), ('Mauritania'), ('Mauritius'), ('Mayotte'), ('Mexico'),");
			commandSql.Append("('Monaco'), ('Mongolia'), ('Montenegro'), ('Montserrat'), ('Morocco'), ('Mozambique'), ('Myanmar'),");
			commandSql.Append("('Namibia'), ('Nauru'), ('Nepal'), ('Netherlands'), ('NewCaledonia'), ('NewZealand'), ('Nicaragua'),");
			commandSql.Append("('Niger'), ('Nigeria'), ('Niue'), ('NorfolkIsland'), ('NorthernMarianaIslands'), ('Norway'),");
			commandSql.Append("('Oman'), ('Pakistan'), ('Palau'), ('Panama'), ('PapuaNewGuinea'), ('Paraguay'), ('Peru'),");
			commandSql.Append("('Philippines'), ('Pitcairn'), ('Poland'), ('Portugal'), ('PuertoRico'), ('Qatar'), ('Romania'),");
			commandSql.Append("('RussianFederation'), ('Rwanda'), ('SaintKittsAndNevis'), ('SaintLucia'), ('SaintMartinFrenchPart'),");
			commandSql.Append("('SaintPierreAndMiquelon'), ('SaintVincentAndTheGrenadines'), ('Samoa'), ('SanMarino'),");
			commandSql.Append("('SaoTomeAndPrincipe'), ('SaudiArabia'), ('Senegal'), ('Serbia'), ('Seychelles'), ('SierraLeone'),");
			commandSql.Append("('Singapore'), ('SintMaartenDutchPart'), ('Slovakia'), ('Slovenia'), ('SolomonIslands'), ('Somalia'),");
			commandSql.Append("('SouthAfrica'), ('SouthGeorgiaAndTheSouthSandwichIslands'), ('SouthSudan'), ('Spain'), ('SriLanka'),");
			commandSql.Append("('StateOfPalestine'), ('Sudan'), ('Suriname'), ('SvalbardAndJanMayen'), ('Swaziland'), ('Sweden'),");
			commandSql.Append("('Switzerland'), ('SyrianArabRepublic'), ('Tajikistan'), ('Thailand'), ('TimorLeste'), ('Togo'),");
			commandSql.Append("('Tokelau'), ('Tonga'), ('TrinidadAndTobago'), ('Tunisia'), ('Turkey'), ('Turkmenistan'),");
			commandSql.Append("('TurksAndCaicosIslands'), ('Tuvalu'), ('Uganda'), ('Ukraine'), ('UnitedArabEmirates'),");
			commandSql.Append("('UnitedKingdom'), ('UnitedStates'), ('UnitedStatesMinorOutlyingIslands'), ('Uruguay'),");
			commandSql.Append("('Uzbekistan'), ('Vanuatu'), ('VietNam'), ('WallisAndFutuna'), ('WesternSahara'), ('Yemen'),");
			commandSql.Append("('Zambia'), ('Zimbabwe')");
			migrationBuilder.Sql(commandSql.ToString());
			commandSql.Clear();
            #endregion

            #region PlaceType
            commandSql.Append("SET IDENTITY_INSERT [dbo].[PlaceType] ON; ");
            commandSql.Append("INSERT INTO [dbo].[PlaceType] ([PlaceTypeId], [Type]) ");
			commandSql.Append("VALUES (1, 'Planet'), (2, 'SpaceStation'), (3, 'City');");
            commandSql.Append("SET IDENTITY_INSERT [dbo].[PlaceType] OFF;");
            migrationBuilder.Sql(commandSql.ToString());
			commandSql.Clear();
            #endregion

            #region Quadrant
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Quadrant] ON; ");
            commandSql.Append("INSERT INTO [dbo].[Quadrant] ([QuadrantId], [QuadrantResource]) VALUES ");
			commandSql.Append("(1, 'QuadrantAlphaResource'), (2, 'QuadrantBetaResource'), ");
			commandSql.Append("(3, 'QuadrantGammaResource'), (4, 'QuadrantDeltaResource');");
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Quadrant] OFF;");

            migrationBuilder.Sql(commandSql.ToString());
			commandSql.Clear();
            #endregion

            #region Timeline
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Timeline] ON; ");
            commandSql.Append("INSERT INTO [dbo].[Timeline] ([TimelineId], [Name]) ");
            commandSql.Append("VALUES (1, 'Prime'), (2, 'Kelvin');");
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Timeline] OFF;");
            migrationBuilder.Sql(commandSql.ToString());
            commandSql.Clear();
            #endregion

            #region Language
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Language] ON; ");
            commandSql.Append("INSERT INTO [dbo].[Language] ([LanguageId], [ResourceName], [CodeISO]) VALUES ");
			commandSql.Append("(1, 'English', 'en'), (2, 'Afar', 'aa'), (3, 'Abkhazian', 'ab'), (4, 'Afrikaans', 'af'),");
			commandSql.Append("(5, 'Amharic', 'am'), (6, 'Arabic', 'ar'), (7, 'Assamese', 'as'), (8, 'Aymara', 'ay'),");
			commandSql.Append("(9, 'Azerbaijani', 'az'), (10, 'Bashkir', 'ba'), (11, 'Belarusian', 'be'), (12, 'Bulgarian', 'bg'),");
			commandSql.Append("(13, 'Bihari', 'bh'), (14, 'Bislama', 'bi'), (15, 'BengaliBangla', 'bn'), (16, 'Tibetan', 'bo'),");
			commandSql.Append("(17, 'Breton', 'br'), (18, 'Catalan', 'ca'), (19, 'Corsican', 'co'), (20, 'Czech', 'cs'),");
			commandSql.Append("(21, 'Welsh', 'cy'), (22, 'Danish', 'da'), (23, 'German', 'de'), (24, 'Bhutani', 'dz'),");
			commandSql.Append("(25, 'Greek', 'el'), (26, 'Esperanto', 'eo'), (27, 'Spanish', 'es'), (28, 'Estonian', 'et'),");
			commandSql.Append("(29, 'Basque', 'eu'), (30, 'Persian', 'fa'), (31, 'Finnish', 'fi'), (32, 'Fiji', 'fj'),");
			commandSql.Append("(33, 'Faeroese', 'fo'), (34, 'French', 'fr'), (35, 'Frisian', 'fy'), (36, 'Irish', 'ga'),");
			commandSql.Append("(37, 'ScotsGaelic', 'gd'), (38, 'Galician', 'gl'), (39, 'Guarani', 'gn'), (40, 'Gujarati', 'gu'),");
			commandSql.Append("(41, 'Hausa', 'ha'), (42, 'Hindi', 'hi'), (43, 'Croatian', 'hr'), (44, 'Hungarian', 'hu'),");
			commandSql.Append("(45, 'Armenian', 'hy'), (46, 'Interlingua', 'ia'), (47, 'Interlingue', 'ie'), (48, 'Inupiak', 'ik'),");
			commandSql.Append("(49, 'Indonesian', 'in'), (50, 'Icelandic', 'is'), (51, 'Italian', 'it'), (52, 'Hebrew', 'iw'),");
			commandSql.Append("(53, 'Japanese', 'ja'), (54, 'Yiddish', 'ji'), (55, 'Javanese', 'jw'), (56, 'Georgian', 'ka'),");
			commandSql.Append("(57, 'Kazakh', 'kk'), (58, 'Greenlandic', 'kl'), (59, 'Cambodian', 'km'), (60, 'Kannada', 'kn'),");
			commandSql.Append("(61, 'Korean', 'ko'), (62, 'Kashmiri', 'ks'), (63, 'Kurdish', 'ku'), (64, 'Kirghiz', 'ky'),");
			commandSql.Append("(65, 'Latin', 'la'), (66, 'Lingala', 'ln'), (67, 'Laothian', 'lo'), (68, 'Lithuanian', 'lt'),");
			commandSql.Append("(69, 'LatvianLettish', 'lv'), (70, 'Malagasy', 'mg'), (71, 'Maori', 'mi'), (72, 'Macedonian', 'mk'),");
			commandSql.Append("(73, 'Malayalam', 'ml'), (74, 'Mongolian', 'mn'), (75, 'Moldavian', 'mo'), (76, 'Marathi', 'mr'),");
			commandSql.Append("(77, 'Malay', 'ms'), (78, 'Maltese', 'mt'), (79, 'Burmese', 'my'), (80, 'Nauru', 'na'),");
			commandSql.Append("(81, 'Nepali', 'ne'), (82, 'Dutch', 'nl'), (83, 'Norwegian', 'no'), (84, 'Occitan', 'oc'),");
			commandSql.Append("(85, 'AfanOromoorOriya', 'om'), (86, 'Punjabi', 'pa'), (87, 'Polish', 'pl'), (88, 'PashtoPushto', 'ps'),");
			commandSql.Append("(89, 'Portuguese', 'pt'), (90, 'Quechua', 'qu'), (91, 'RhaetoRomance', 'rm'), (92, 'Kirundi', 'rn'),");
			commandSql.Append("(93, 'Romanian', 'ro'), (94, 'Russian', 'ru'), (95, 'Kinyarwanda', 'rw'), (96, 'Sanskrit', 'sa'),");
			commandSql.Append("(97, 'Sindhi', 'sd'), (98, 'Sangro', 'sg'), (99, 'SerboCroatian', 'sh'), (100, 'Singhalese', 'si'),");
			commandSql.Append("(101, 'Slovak', 'sk'), (102, 'Slovenian', 'sl'), (103, 'Samoan', 'sm'), (104, 'Shona', 'sn'),");
			commandSql.Append("(105, 'Somali', 'so'), (106, 'Albanian', 'sq'), (107, 'Serbian', 'sr'), (108, 'Siswati', 'ss'),");
			commandSql.Append("(109, 'Sesotho', 'st'), (110, 'Sundanese', 'su'), (111, 'Swedish', 'sv'), (112, 'Swahili', 'sw'),");
			commandSql.Append("(113, 'Tamil', 'ta'), (114, 'Telugu', 'te'), (115, 'Tajik', 'tg'), (116, 'Thai', 'th'),");
			commandSql.Append("(117, 'Tigrinya', 'ti'), (118, 'Turkmen', 'tk'), (119, 'Tagalog', 'tl'), (120, 'Setswana', 'tn'),");
			commandSql.Append("(121, 'Tonga', 'to'), (122, 'Turkish', 'tr'), (123, 'Tsonga', 'ts'), (124, 'Tatar', 'tt'),");
			commandSql.Append("(125, 'Twi', 'tw'), (126, 'Ukrainian', 'uk'), (127, 'Urdu', 'ur'), (128, 'Uzbek', 'uz'),");
			commandSql.Append("(129, 'Vietnamese', 'vi'), (130, 'Volapuk', 'vo'), (131, 'Wolof', 'wo'), (132, 'Xhosa', 'xh'),");
			commandSql.Append("(133, 'Yoruba', 'yo'), (134, 'Chinese', 'zh'), (135, 'Zulu', 'zu'), (136, 'EnglishUSA', 'en-US'),");
			commandSql.Append("(137, 'PortugueseBrazil', 'pt-BR'); ");
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Language] OFF; ");
            #endregion

            #region CharacterClassification
            commandSql.Append("SET IDENTITY_INSERT [dbo].[CharacterClassification] ON; ");
            commandSql.Append("INSERT INTO [dbo].[CharacterClassification] ([CharacterClassificationId], [Classification]) ");
            commandSql.Append("VALUES (1, 'Humanoid'), (2, 'Andoid') ");
            commandSql.Append("SET IDENTITY_INSERT [dbo].[CharacterClassification] OFF;");
            migrationBuilder.Sql(commandSql.ToString());
            commandSql.Clear();
            #endregion

            #region CharacterClassification
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Role] ON; ");
            commandSql.Append("INSERT INTO [dbo].[Role] ([RoleId], [RoleResource]) VALUES ");
            commandSql.Append("(1, 'Creator'), (2, 'Direcotr'), (3, 'Writer'), (4, 'Producer'), (5, 'Actor') ");
            commandSql.Append("SET IDENTITY_INSERT [dbo].[Role] OFF;");
            migrationBuilder.Sql(commandSql.ToString());
            commandSql.Clear();
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role]");
            migrationBuilder.Sql("DELETE FROM [dbo].[CharacterClassification]");
            migrationBuilder.Sql("DELETE FROM [dbo].[Language]");
            migrationBuilder.Sql("DELETE FROM [dbo].[Timeline]");
            migrationBuilder.Sql("DELETE FROM [dbo].[Quadrant]");
            migrationBuilder.Sql("DELETE FROM [dbo].[PlaceType]");
            migrationBuilder.Sql("DELETE FROM [dbo].[Country]");
        }
    }
}
