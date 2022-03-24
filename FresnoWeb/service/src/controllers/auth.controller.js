const Helpers = require('../helpers/helpers');

const UserRepository = require('../repositories/user_repository');
const RoleRepository = require('../repositories/role_repository');
const UserRoleRepository = require('../repositories/userrole_repository');

const config = require('../config/auth.config');
var jwt = require('jsonwebtoken');
var bcrypt = require('bcryptjs');

class AuthController {
    constructor(db) {
        this.db = db;
        this.userRepository = new UserRepository(db);
        this.roleRepository = new RoleRepository(db);
        this.userRoleRepository = new UserRoleRepository(db);
        console.log(`${Helpers.getDateNowString()} HELLO from AuthController constructor`);
    }

    async signup(req, res) {
        // Save User to Database
        await this.userRepository.create(req.body.username, req.body.email, bcrypt.hashSync(req.body.password, 8))
            .then(user => {
                if (!user) {
                    throw new Error("User was not created!");
                }

                if (req.body.roles) {
                    this.roleRepository.findByNames(req.body.roles)
                        .then(roles => {
                            roles.forEach(element => {
                                this.userRoleRepository.insertNewUserRole(res, element.Id, user.Id)
                            });
                            res.send({ message: "User was registered successfully!" });
                        });
                } else {
                    // user role = 1
                    this.userRoleRepository.insertNewUserRole(res, 1, user.Id).then(() => {
                        res.send({ message: "User was registered successfully!" });
                    });
                }
            })
            .catch(err => {
                res.status(500).send({ message: err.message });
            });
    }

    async signin(req, res) {
        await this.userRepository.findByUsername(req.body.username)
            .then(user => {
                if (!user) {
                    return res.status(404).send({ message: "User Not found." });
                }

                var passwordIsValid = bcrypt.compareSync(
                    req.body.password,
                    user.Password
                );

                if (!passwordIsValid) {
                    return res.status(401).send({
                        accessToken: null,
                        message: "Invalid Password!"
                    });
                }

                var token = jwt.sign({ id: user.Id }, config.secret, {
                    expiresIn: 86400 // 24 hours
                });

                this.userRoleRepository.findByUserId(user.Id).then(roles => {
                    const rolePromises = roles.map(async userRole => {
                        const role = await this.roleRepository.findById(userRole.RoleId);
                        return `ROLE_${role.Name.toUpperCase()}`
                    });
                    Promise.all(rolePromises).then(authorities => {
                        res.status(200).send({
                            id: user.Id,
                            username: user.Username,
                            email: user.Email,
                            roles: authorities,
                            accessToken: token
                        });
                    });

                });
            })
            .catch(err => {
                res.status(500).send({ message: err.message });
            });
    }
}

module.exports = AuthController;