EXEC SP_RENAME 'ReferenceType.TypeName', 'TypeName_en'  
ALTER TABLE [dbo].[ReferenceType] ADD [TypeName_es] [nvarchar](25)

INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201602232223109_AutomaticMigration', N'EBinder.Models.EBinderContext',  0x1F8B0800000000000400DD5DDB6E1CB9117D0F907F18CC5312783592D70B6C0C6917BB929D08F10D92779337A13D438D1BDBD333E9EE312404F9B23CE493F20B61DF79A9228BECAB8C0516D6F056AC3A2C9255D5ACFFFDE7BFE73F3EECA2C51796A4E13EBE589E9D9C2E172C5EEF3761BCBD581EB3FB6FBE5FFEF8C3EF7F77FE6AB37B58FC5AD7FB36AFC75BC6E9C5F273961D5EAE56E9FA33DB05E9C92E5C27FB747F9F9DACF7BB55B0D9AF9E9F9EFE797576B662BC8B25EF6BB138BF39C659B863C51FFCCFCB7DBC6687EC18446FF71B16A5D5EFBCE4B6E875F12ED8B1F410ACD9C5F2D5CF61BC61C9495973B9F8290A034EC52D8BEE978B208EF75990711A5FFE92B2DB2CD9C7DBDB03FF21883E3E1E18AF771F4429AB687FD956A74EE3F4793E8D55DBB0EE6A7D4CB3FDCEB1C3B36F2BBEACD4E65EDC5D367CE39C7BC5399C3DE6B32EB877B1BC6451B45CA803BDBC8C92BC92CAD893BCFAB345F5E3B346F41C21F97FCF1697C7283B26EC2266C72C0978D50FC74F51B8FE1B7BFCB8FF8DC517F1910F2710C449E265D20FFCA70FC9FEC092ECF186DD0B645E5F2D172BB9ED4A6DDC3455DA9593B98EB36F9F2F17EF3811C1A788357217267E9BED13F61716B324C8D8E64390652CE162BBDEB082731A05CA78F9FFEBD138D0F87A592EDE060F6F58BCCD3E5F2C9F7FB75CBC0E1FD8A6FEA122E09738E4AB8BB7C992230308340FFAF77DF2DB9AD3C712C3D067A7A4A1CD235DB14390643B3E969DA94A4FEF822FE1B6E031DAE77271C3A2A24AFA393C94ABF7A42DBECB85C9D7F6EB64BFBBD94752D3B2ECEE63906C19EFE7E31EA970BB3F266B07E2380259C2551F4B41E29A629D36A548234D2D77A58C6BB204262A2FB92BD7754B4EF3A346485B029170BE6A55865191886224AB93B6D1144A4546B3AB6A71590B4F5CC17C88823E577CB556BA2EF61AAEE862AF914EA5AB982648575172D70E2011A6156A4B4CAF31981E4268042B1874928D56B25AA8784AD60845FD2994418371573D405C1C4F5C05DCB02D27DD5907A82B7F1F67C13A7BB50BC2C83005FECF21E6F02688B7C7606B621E7564DB347787207E7C77DC7D2A36E9BE87239CA5603DE1A8C7540D8B6B3A2A817FDDEFD8072E02AA3ECB3977570C5BB704B59A5E0DD76D40DDC1B471310CA288CB32830EAE2A781357DC6D0D30502BAA4890CB113028955CF150AA15848B7911C842A100E09F58DA69EF92E6E6B08749EDA6D8CB24027CF634AD83B1F6B67C9CFC5F772C1E7D8B6BC736A96CE2D8B6DDB4E2EF25E7D2769F3CF677B85694E530CA55D5051445EC34811EF5167C1847949BB392AD0548E034D006E3B656D5C271BD3EC47577C5D752EAAEFCEAB6932A40717D792B41FA22ED4F11D663DA95E10B9A61CFF5A42E8D6F5288C4F1BB01D107809302AF13E046DD71C32C32DD88CEBE1B045FE5DDC834EE8B2186FDE5E68D716F1F64AEAFA3607BC585538F9CFFFB63B8F3B9843B1DCBBADBE31CECDBE806E5698CB35D653D4D5E38995DAEB51F784BEB5DB0AC035E05F322036965798F0729A71BAA812EF90A46252C91EF5546FA94BA1099521503B5723DBF9BAA8D9D4D2D88D0AAD040625DA3D3E92D478B8BC595579FC4E0CAC7F5B2B756EDC6DA20F3F1AE58BA1ED18C872BE49C185D17B7BFEA569AB6C81DF1542B974DB3196C5C92E6F33C23962624870362DE609AD3616D44773F1AD2CCEF4FCDCBE0B779910C83BA9685CC86BD2F06D20E802F08758FF05A12B94BDF6141E4D5A7580EF9B8710133D7E5D0B6C4EF0FCFBF1FE45E4C0A28B25C4238F5FD9FE07D6241D435A24789502938567342A9280E5D55D08A4C4A5B02C6A608C59D1785A321BF6E32D5E2F035DFD30136DA7ED1F711C91CFE44021B047E008B14B0FD94A6FB755850A25D5FABE39A3CAF57F166610B6B29592987C5709E72BC85078E304E06D7724B1549EFE32B16B18C2D7E5A9711A79741BA0E368092E53490896A16664B54A9556472FEA48D52ED6C79ACED2517135F22619CE9E80FE3757808220B4F9476CE415DF9AC9BB1D412DE90C539D82D3CE88388662C4524366E9DAF04A49901A8BBFA31611BFCFEADB4AB389E51D06708A8A2AC899E4088B2850200249AC80980281B3A123002F84C2E464CEA247F632B7FC5396F83006D1C006720F0FFA46E0B9E9CD11D6D34AA0DFEC1DE3984FB16A1B15A8FE180AAC26B3D136745595C04F7A2D34A270AB747D2C6D401960310EA4800D0E585E24E4718AF0953765D8B7BA2B7C9F7B58DBA1C3D5A2325BAC503164B41E716166C999CD3939333C384013BE708822E2D9F7621280E9EDE84AB9857CD1CEC65C2A5D18D40996284EB6FCA4AD8DFF03BB5E283B2938838A47A6301ECC8A2EEFC33DA89E199386D73B089C46FF78505D71339A3ECB8A2611CC711682517D1533A5446B9EB21C1BC6EABDA1F88002B68F2861D398EB803A6DE75F4510F76B5AFC3AEB234D7776FCA5073A9D880DCE15C03859F9A8D184898A93BBE2D1DBB6E02FE4C683D0A1881807BA1A5AA744DD1670B7CB76A3ED64DBFBD692453D634FC81BB933ED1F8DE6DDC11348962AA376100B2DBCBA81AFCAC831204209306736F78E9DCA0C81A772939E34C9F7D77027A075CE9AB29BE390C6396544454AEBCFC67F600054A712A2B87605A79A5541CE4DDDEB24C8ED6693D439276D2349BDC580A5ED4BA106FE7968EEA7008AD8F6A83B134577635BD1B653BA176D7DA03F11EDB3AD45E8D04DA5955C62BE99C2A2ECD561AAAF80E8080F2B861E9A0724B6ACD4B9541688C49A855824A27C2F2808057C79309D5B0CFF1D5354B7063363350B0AEAD7F82F751E8ABA25955DAF254096C00BE88D5F960F1A5599C570027EAC56A6002EE04A371D4831726AF0DC015B293C7D9CD23CC50D54B068E511C3B561974661BA0D56CACB37881BCFC407DB010F7FC40BDA35ABC034771D564F42E90FC0BD01C88DC195E2DC1BE04231F68EAC9EC76E8C29351559518070DE86BC40161734188DAA13C239814B4EE711814081545260800EE089243A28BD86517848D83DD3850696C230B00E327CD45D18909B2F972D05D46FD24C6C40D83E7C2C977D18537B0B782BC3F79B14A0A108718841ACA09A67289F4EA3260E40464E11E16220D5D267080D65BA2FDB60B20548BAD9D9FDEE77A155AD8D1DE64D3A55B757D0EF8881D77C8E521C484EBFC40CCBB1603AF406E75BD354C5C37E90EB06FAA51C0F04C31E323C5FCA8CCD97EB846AC8636DE19265F87233726AEA6EC7C55BE915BFD70BE421ED33D7F1B1C0E61BC151ED7AD7E59DC962FEB5E7E73EBFEECECAEEC63B596F8AA1AE49A91B27DC22F694A69FE8CD686BD0E9334BB0AB2E05390478B5F6E765A35C5A0871850EAC1449B9D2EABDAA452D7CEFF2D990DC58770019B67D5EE359F4F7ED42DA6C61478EBCD16F993C6411424C8C72697FBE8B88BCD067BBC9732325FECE31DF0D18DA907F18159B11FF1777A6F7270B4D89F396C3A87B1C25FCD86AC495033EACB702081C57CB322434630E6BA03C7D4B87F460F07A5262E59EC040D569E4CE4E8D1902CEDD2ECEE2E68A45D5FFC1C4EB26D5482D80B1EAB80F724BF1229293FA984DE63FBF2A3D85BFBAB0B6DF2D38E327572D96CD06C3DDE92517D63BA5012D06D698F634B89AF92216609BEC2FBADE759BCB024F62915F8F49762FD81B8B0CE5B8CE606E76E0AF79E1E7706C3B63BF4B0CE5CE087F731B42850C5A23EF8256916B5D0B75F5563A985F3834E3FEAAA0B563C308262C3513D956F73498AA4FCC96163AF1EDA92B6F6EA377A2FC5BB596217C50FF4F6ED13586227EDAF1E2AD1732B98EA408998E6E9E7C93C38C1E3380936434F93D5DB3AD261127CA7C7D64BF9628EDA4FF9EB6C84825B401D344C11F4E1A35EE086C31FAFE947FD89C48219E4C842294269DC450237435562F30689A41791374D4C3DF563E61183EB548A66A70E5B8B6947317BDE43F0A6FDB277FE6B50362E1BEE8D6A4010ED8EA8B6325E0841DB476E41379D01E018229D8F1D4F1377D6B34511325CF7DF6D12684CADA30DE8CEF00C0046ACEA6FF0474D1DA061474A5D13BFAF211E298CA7B677437C8FEF9BFEC46E7B55C46DAB7022AC47114B613D04414BF55171E3D66E2347498F75CC41F4A4473DFC6DFAE382418CC5325E5FB0554E5CDC7AC016553F22B79A3BEC6AE328573D12ACEB6A76A5E9721F6FC2E2C39AEB347FE3AB795D8B38E51E154215F6455005554D5409405757E3AA9282DAE6B9F0A568B98EF09D4CE7D7C12D141957557121BB1EF9AC5FEBCE42CC966F7A5D5D795349BA89C72288BAA98BCA1A36791819A9C49DCD53DA4A401B9948834167E41D1C0C9633BBB6B5EAA023DBE8EF83B652437CDDC8CB884E9FABDCC906EC9E51A0858BA9551A5B43F54BF377132E56856AD913B26BB15B6595E582D3FE25DCE4715BB78F69C67627798593DB7F469751989FF79B0A6F8338BC6769563EA3BA7C7E7AF65CC9EB3E9F1CEBAB34DD48475824D1BA2CB011339F873967AD2FBCBAE67009DA275FE32F41B2FE1C24DA23E19D7399833DE7D9CCDB9E7D539787606ADA6BAEEB1E2E96FF2A1ABE5C5CFFE34E6CFB6CF13EE1E87CB9385DFCDB3D91B2780477A341688A91D021D11276E19E2A91F693C1AB92FD972CCDB29903981C332E7B48B1AF0CC84F46766A5205AAF0EA769D5401940AB99ED81F76C1C31F3BA737467BF34F61ECD0A56796550FDC5A0CF3DED94E07C1B11458D62F9CA5183372D71DD395D2D78CD6C510CACF12E0358FEC9183000B49180922E0C569C7EECD007B61399C79A47FEC22C59E52310EA30EC4EC8BF049F73B7761C9C915E16E5F74C99D88E815774AD5D4881BFEEFACC7D488CEEAA96C3EC8B94C33678F99276D10F4AAA9D186391EE846C3B1B36DCDF444EB960FCA8383FD25688275509EA2A953FE25EAF22E5B753AAFEB3975A883B72D87502B7024D3143986465B2583DD3EBCAFCEAE9142C8E51B0BD771BCC2E80474D80C3B6C8D88A5E0AE8BCD6448CB8921DAC8F5C80959EE11DBAE56D5E574BAF196EDF5A69BF9A222DD430FBB0CEC2EBB6E964C57099A826E9E841CE509789BC58795A9EFD1D9559A7054C59390634DBABB536168D975DE53A9C2835DBC4F437A35ED3E5E8461E5D7E5FEE526402CF4E14948B025DECB9730880C6D11182E6E21AA088DE10B5457D2080BC16641F20752D9BC0779F6925474DA3CA2E52B530205A3240D1D374F28F21290FB88F34B0A3A59065067FCF699F373B4049F8E889D6D4A4F30400F7B5578A2445508353ACCA1E281920F999F527176743B61CF66F7A29040F9246E626442A199B3D1727341E2B85ACF1379D6EF1947479A2DD6001074FB463A24E75172E77656CB7DAB3FC3733E5EA119FE60EC9382F113EC9277615AF6B5A141A75D0D94926141E726EABE4046BE2A984D275380CBB291922E7A5F25A4C832ED21C39B3B90F06FC347C590EDC3ECF9AA29E4260A967F8D2ACBF18E3A27C565FBC87E86F6B2F9C06E2A6B9A2FF06CAF1F8C615CA3A5BDEF9AE97E4C9B2AF2E81C354871183BAAE5E98651750CFEE4C07CB7340D844AC9D7B88D91813CA70D0C7F2CC255A57C95901A5339B903097FA1635C0CE1AF65CC5843E1B6D0512CF213E928772BFC7C00F6340CED13016B2203BBEBB0D383293104ABCCDE9F48D39D5F97F7B0AF914741199AF44FCBD6A788B2FC6914341115656F28425EAFA67F4DE5841B972D0E1F6E5495647A816ABE272810C64AD9D778867202F47C0E51A617C466A49CE600AF7115961FB0F027DC468BF193F65CABF69AF6780EC4296BA4184F764FF2986E7F1F6F864775F2FB77B33DAECF056CD31EE03DC12793301508911CC5330947283EB617C72E7F78F2C10748521597B146C246119D0724910172316B821A51138D091443AA1497670186018C79BCDE41F34A7E04B546AD966C5A955BF5B2AD80F23A79B7A668CAC74E2F969B4F7B2EE1F2CB9FB24473EBCABD8A7E7DAD6FB1101A412CB78C531DEFB421AADFA1DEAB224BC7CAD6A90DA09443032955A803B6D1B3F8A06D1DE3C06D35EAE0A6412D83D96555B8D07451153F83922A4AACA497D73480EEB20026BA2CB3F45D2A32ADE7F267A8DFB284D02B82AAB608EBDD0D4B42C4BD19C186F502D5236319562E4A39A6659A6A5A424F7C44A3E2016B9935900F0D08D095720CF15E23DA646CD3873E831A569E52035F820EE342371A58EF137473FD618D617CF91572ECCBD585504DDBDEA0282AE9E8A4A3B578721EDF0081DB81D00EDC9A95E7D40993D4BFAE046669F9041332CB0894827AA41B6B3C262A0105FE16109839A1153E2D10C0404A71136B70F58EA5A21B965595DDC4915570B6818EA8999E35C0E1CBC617BD096182EA3111C90B4E66161D8B5D58247FE263E28C314316ECB08226406380A6499592DE19405832C64C5CAEBBC0E4D306BE9C304EDE9E02AF6F0C201B0E583E106328A8B0A7DCEBED7C310D6BC40C7BC0B103094E97770EE100DE660F9FE228D5CA4D8AB0364AD894FEAD6FD06B9C524A7A670005E2A6E4729DA43CC9B4A5C055F3BCF118D701248F1F8E7A3F1829C19514260C7B3A1C75F2897C98324D5EAE3AD22D8286222F1688916AE0C4D150368554D1D250D10959F07C10D249B64A549551B8E61C807DAF6F90674AD9008CA02C71739EC1CE721F9705A63015D46C0257EF57D9E1F6322CADE140CCC021E19085B067E53705735A773FC00A2416A0E34D4F7460142D400785DF5404EF34321FCC7F8D78B01532AD32EC3A392D8D645376BE2A6DC6D50FFC4F2D5DE4F9EAE618E7193ECABFAE581A6EDB2EF244D6315B4B1EDBA6CE757CBFAF9DC70A457515E549C7B72C0B364116FC9464E17DB0CE78F19AA569186F978B5F83E8C8ABBCDA7D629BEBF8FD313B1C333E65B6FB1449D69FDC016D1AFF7CA5D17CFEFE90FF95F631054E66982745791FFF7C0CA34D43F76BE01D49A48BDCB35DBDC89FCB32CB5FE6DF3E363DBDDBC7C48E2AF6350EF98F6C77887867E9FBF836F8C27C68E3D07BC3B6C1FAF14395F513EFC42E0899EDE75761B04D825D5AF5D1B6E77F720C6F760F3FFC1F5EC9BD45ECF20000 , N'6.1.3-40302')
