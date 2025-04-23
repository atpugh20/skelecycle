class Vector {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }

    add(other_vector) {
        this.x += other_vector.x;
        this.y += other_vector.y;
    }

    sub(other_vector) {
        this.x -= other_vector.x;
        this.y -= other_vector.y;
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