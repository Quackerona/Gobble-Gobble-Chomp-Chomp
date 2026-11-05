// Bunch of unstructed hardcoded god-file mess but I genuinely just needed something out. Plus this is just for testing raylib as opposed to a professional project.

using System.Numerics;
using Raylib_cs;

// Window stuff.
Raylib.SetConfigFlags(ConfigFlags.HighDpiWindow);
Raylib.InitWindow(800, 800, "Gobble Gobble Chomp Chomp");
Raylib.SetTargetFPS(60);

Raylib.InitAudioDevice();

// Main game content.
Texture2D title = Raylib.LoadTexture("assets/images/title.png");
Texture2D bg = Raylib.LoadTexture("assets/images/bg.png");
Texture2D shine = Raylib.LoadTexture("assets/images/shine.png");
Texture2D hotDog = Raylib.LoadTexture("assets/images/hot dog.png");
Texture2D end = Raylib.LoadTexture("assets/images/end.png");

Sound chomp = Raylib.LoadSound("assets/sounds/chomp.mp3");

Font font = Raylib.LoadFont("assets/fonts/trebuc.ttf");

GameState gameState = GameState.START;

// Stupid numeric stuff.
float timeLeft = 30.0f;

int hotdogsEaten = 0;
float hotdogScale = 1.0f;

float shineRotation = 0.0f;
float shineAlpha = 1.0f;
bool shineTranslucencyFlip = false;

while (!Raylib.WindowShouldClose())
{
    float deltaTime = Raylib.GetFrameTime();

    shineRotation += 10.0f * deltaTime;
    if (!shineTranslucencyFlip)
    {
        shineAlpha -= deltaTime * 0.6f;
        if (shineAlpha <= 0.4f) shineTranslucencyFlip = true;
    }
    else
    {
        shineAlpha += deltaTime * 0.6f;
        if (shineAlpha >= 1.0f) shineTranslucencyFlip = false;
    }

    if (hotdogScale > 1.0f) hotdogScale -= deltaTime * 0.6f;

    switch (gameState)
    {
        case GameState.START:
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                gameState = GameState.PLAYING;
            break;
        case GameState.PLAYING:
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                hotdogsEaten += 1;
                hotdogScale = 1.05f;

                Raylib.SetSoundPitch(chomp, (float)Random.Shared.NextDouble() * (1.0f - 0.75f) + 0.75f);
                Raylib.PlaySound(chomp);
            }

            timeLeft -= deltaTime;

            if (timeLeft <= 0.0f) gameState = GameState.STOPPED;
            break;
        case GameState.STOPPED:
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                gameState = GameState.PLAYING;
                timeLeft = 30.0f;
                hotdogsEaten = 0;
            }
            break;
    }

    int timeLeftDisplay = (int)MathF.Ceiling(timeLeft);
    Vector2 timeTextSize = Raylib.MeasureTextEx(font, $"TIME LEFT: {timeLeftDisplay}", 50, 1.0f);

    Vector2 hotdogTextSize = Raylib.MeasureTextEx(font, $"HOTDOGS EATEN: {hotdogsEaten}", 50, 1.0f);

    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(Color.Black);

        Raylib.DrawTextEx(font, $"TIME LEFT: {timeLeftDisplay}", new Vector2((800 - timeTextSize.X) / 2, 120.0f), 50, 1.0f, Color.White);
        Raylib.DrawTextEx(font, $"HOTDOGS EATEN: {hotdogsEaten}", new Vector2((800 - hotdogTextSize.X) / 2, 600.0f), 50, 1.0f, Color.White);

        Raylib.DrawTexture(bg, 0, 0, new(1.0f, 1.0f, 1.0f, 0.3f));
        Raylib.DrawTexturePro(shine, new Rectangle(0.0f, 0.0f, 464.0f, 464.0f), new Rectangle(400.0f, 400.0f, 464.0f, 464.0f), new Vector2(232.0f, 232.0f), shineRotation, new Color(1.0f, 1.0f, 1.0f, shineAlpha));
        Raylib.DrawTextureEx(hotDog, new Vector2(251.0f, 291.5f), 0.0f, hotdogScale, Color.White);

        if (gameState == GameState.START)
        {
            Raylib.DrawRectangle(0, 0, 800, 800, new Color(0.0f, 0.0f, 0.0f, 0.9f));
            Raylib.DrawTexture(title, 0, 0, Color.White);
        }
        else if (gameState == GameState.STOPPED)
        {
            Raylib.DrawRectangle(0, 0, 800, 800, new Color(0.0f, 0.0f, 0.0f, 0.9f));
            Raylib.DrawTexture(end, 0, 0, Color.White);
            Raylib.DrawTextEx(font, $"HOTDOGS EATEN: {hotdogsEaten}", new Vector2((800 - hotdogTextSize.X) / 2, 350.0f), 50, 1.0f, Color.White);
        }
    }
    Raylib.EndDrawing();
}
Raylib.UnloadTexture(title);
Raylib.UnloadTexture(bg);
Raylib.UnloadTexture(shine);
Raylib.UnloadTexture(hotDog);
Raylib.UnloadTexture(end);
Raylib.UnloadSound(chomp);
Raylib.UnloadFont(font);
Raylib.CloseWindow();

enum GameState
{
    START,
    PLAYING,
    STOPPED
}
