const Helpers = require('../helpers/helpers');

// Database repositories
var RoleRepository = require('../repositories/role_repository');
var UserRepository = require('../repositories/user_repository');

class VerifySignUpMW {
    constructor(db) {
        this.db = db;
        this.roleRepository = new RoleRepository(db);
        this.userRepository = new UserRepository(db);
        console.log(`${Helpers.getDateNowString()} HELLO from VerifySignUpMW constructor`);
    }

    checkDuplicateUsernameOrEmail(req, res, next) {
        // Username
        this.userRepository.findByUsername(req.body.username)
            .then(user => {
                if (user) {
                    console.log(`${Helpers.getDateNowString()} Username: ${user.Username} is already existing`);
                    res.status(400).send({
                        message: "Failed! Username is already in use!"
                    });
                    return;
                }

                // Email
                this.userRepository.findByEmail(req.body.email)
                    .then(user => {
                        if (user) {
                            console.log(`${Helpers.getDateNowString()} Email: ${user.Email} is already existing`);
                            res.status(400).send({
                                message: "Failed! Email is already in use!"
                            });
                            return;
                        }
                    });
                next();
            });
    };

    async checkRolesExisted(req, res, next) {
        if (req.body.roles) {
            await this.roleRepository.findAll().then(roles => {
                const allRoleNames = roles.map(r => r.Name);
                for (let i = 0; i < req.body.roles.length; i++) {
                    if (!allRoleNames.includes(req.body.roles[i])) {
                        res.status(400).send({
                            message: `Failed! Role does not exist = ${req.body.roles[i]}`
                        });
                        return;
                    }
                }
            });
        }
        next();
    };
}

module.exports = VerifySignUpMW;