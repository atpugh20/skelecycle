class Vector {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }

    add(other_x = 0, other_y = 0) {
        this.x += other_x;
        this.y += other_y;
    }

    sub(other_x = 0, other_y = 0) {
        this.x -= other_x;
        this.y -= other_y;
    }

    mult(scalar = 1) {
        this.x *= scalar;
        this.y *= scalar;
    }

    div(scalar = 1) {
        if (scalar == 0) {
            console.log("Cannot divide by zero!");
            scalar = 1;
        }

        this.x /= scalar;
        this.y /= scalar;
    }

    getMag() {
        return Math.sqrt(this.x * this.x + this.y * this.y);
    }

    normalize() {
        this.div(this.getMag());
    }

    clone() {
        return new Vector(this.x, this.y);
    }
}