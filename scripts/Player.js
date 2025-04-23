class Player {
    constructor(x, y) {
        this.pos = new Vector(x, y);
        this.vel = new Vector(0, 0);
        this.acc = new Vector(0, 980);

        this.speed = 100;

        this.jumping = false;
    }

    update(delta_time) {
        /**
         * Updates using Velocity Verlet:
         *  Pf = Pi + v * dt + (a * dt) / 2
         */

        let vel_clone = this.vel.clone();
        let acc_clone = this.acc.clone();

        vel_clone.mult(delta_time);
        acc_clone.mult(delta_time);
        acc_clone.div(2);

        this.pos.add(vel_clone);
        this.pos.add(acc_clone);

        if (this.pos.y >= 595) {
            this.jumping = false;
            this.pos.y = 595;
            this.vel.y *= -0.9;
        }

        acc_clone = this.acc.clone();
        acc_clone.mult(delta_time);
        this.vel.add(acc_clone);
    }

    draw(ctx) {
        ctx.fillStyle = "white";
        ctx.fillRect(this.pos.x, this.pos.y, 5, 5);
    }

    handleMovementKey(event) {
        let movement = 0;
        if (event.type == "keydown") {
            movement = this.speed;
        }

        switch (event.code) {
            case "KeyA":
                this.vel.x = -movement;
                break;
            case "KeyD":
                this.vel.x = movement;
                break;
            case "Space":
                if (!this.jumping) {
                    this.vel.y = -movement * 10;
                    this.jumping = true;
                }
                break;
            default:
                console.log(event.code);
                break;
        }
    }

    handleMovementKeyDown(event) {
    }

    handleMovementKeyUp(event) {
        console.log(event);


    }
}