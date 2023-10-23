
class NoToken extends Error {
    status: number;
    constructor() {
        super('No token');
        this.name = 'NoToken';
        this.status = 401;
    }
}
class InvalidRefreshToken extends Error {
    status: number;
    constructor() {
        super('Invalid refresh token');
        this.name = 'InvalidRefreshToken';
        this.status = 403;
    }
}
class InvalidToken extends Error {
    status: number;
    constructor() {
        super('Invalid token');
        this.name = 'InvalidToken';
        this.status = 403;
    }
}
class MissingParameters extends Error {
    status: number;
    constructor(msg : string) {
        super('missing: ' + msg);
        this.name = 'MissingParameters';
        this.status = 401;
    }
}
class InvalidPasswordOrUserNotFound extends Error {
    status: number;
    constructor() {
        super('Invalid password or username');
        this.name = 'InvalidPassword';
        this.status = 404;
    }
}

class JWTExpired extends Error {
    status: number;
    constructor() {
        super('jwt expired');
        this.name = 'JWTExpired';
        this.status = 403;
    }
}


const Errors = {
    NoToken,
    InvalidRefreshToken,
    InvalidToken,
    MissingParameters,
    InvalidPasswordOrUserNotFound,
    JWTExpired
}

export default Errors;