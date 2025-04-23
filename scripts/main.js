let last_frame_time = 0;
let delta_time = 0;

const player = new Player(50, 50);

function setup() {
    // Runs before rendering starts

}

function draw(current_time) {
    // Runs every frame
    clearCanvas();
    delta_time = (current_time - last_frame_time) / 1000;
    last_frame_time = current_time;

    player.update(delta_time);
    player.draw(ctx);

    requestAnimationFrame(draw);
}

function main() {
    setup();
    requestAnimationFrame(draw);
}

["keydown", "keyup"].forEach(
    (type) => document.addEventListener(type,
        (e) => player.handleMovementKey(e)
    )
);

main();