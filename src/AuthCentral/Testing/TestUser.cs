using System;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    internal class TestUser : HierarchicalUserAccount
    {
        private static TestUser _testUser;
        private static TestUser _testAdmin;

        public static TestUser PreloadUser
        {
            get
            {
                if (_testUser != null)
                {
                    return _testUser;
                }

                _testUser = new TestUser
                {
                    Created = new DateTime(2015, 11, 19, 16, 18, 47),
                    Email = "preload@fsw.com",
                    HashedPassword = "C350.AHGxOLvg8C41MoLx9kGPtToyIZs8A1ZO1mvDVzy3q27Uvolvn37yhmVaLDZTX10Hhw==",
                    ID = Guid.Parse("243dbc98-e273-43fc-a0d7-2976c939b1f0"),
                    IsAccountClosed = false,
                    IsAccountVerified = true,
                    IsLoginAllowed = true,
                    LastLogin = new DateTime(2015, 11, 19, 16, 20, 13),
                    LastUpdated = new DateTime(2015, 11, 19, 16, 20, 13),
                    PasswordChanged = new DateTime(2015, 11, 19, 16, 18, 47),
                    RequiresPasswordReset = false,
                    Tenant = "fsw",
                    Username = "Preload"
                };

                _testUser.AddClaim(new UserClaim("scope", "fsw_platform"));

                return _testUser;
            }
        }

        public static TestUser TestAdmin
        {
            get
            {
                if (_testAdmin != null)
                {
                    return _testAdmin;
                }

                _testAdmin = new TestUser
                {
                    Created = new DateTime(2015, 11, 19, 16, 18, 47),
                    Email = "admin@fsw.com",
                    HashedPassword = "C350.AHGxOLvg8C41MoLx9kGPtToyIZs8A1ZO1mvDVzy3q27Uvolvn37yhmVaLDZTX10Hhw==",
                    ID = new Guid(),
                    IsAccountClosed = false,
                    IsAccountVerified = true,
                    IsLoginAllowed = true,
                    LastLogin = new DateTime(2015, 11, 19, 16, 20, 13),
                    LastUpdated = new DateTime(2015, 11, 19, 16, 20, 13),
                    PasswordChanged = new DateTime(2015, 11, 19, 16, 18, 47),
                    RequiresPasswordReset = false,
                    Tenant = "fsw",
                    Username = "Admin"
                };

                _testAdmin.AddClaim(new UserClaim("scope", "fsw_platform"));
                _testAdmin.AddClaim(new UserClaim("fsw:authcentral:admin", "true"));

                return _testAdmin;
            }
        }
    }
}