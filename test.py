from playwright.sync_api import sync_playwright

BASE_URL = "https://top.gg/tag/antinuke"

def get_antinuke_bots():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)  # Run headless (no UI)
        page = browser.new_page()
        page.goto(BASE_URL, timeout=60000)  # Load the page

        # Wait for bot elements to load
        page.wait_for_selector("div[data-testid='bot']")

        bots = []
        bot_elements = page.query_selector_all("div[data-testid='bot']")

        for bot in bot_elements:
            name = bot.query_selector("h4").inner_text().strip() if bot.query_selector("h4") else "Unknown Bot"
            invite_link = bot.query_selector("a[href*='/bot/']")
            bot_id = invite_link.get_attribute("href").split("/bot/")[1] if invite_link else "Unknown ID"
            
            if invite_link:
                bot_url = "https://top.gg" + invite_link.get_attribute("href")
                bots.append({
                    "name": name,
                    "bot_url": bot_url,
                    "bot_id": bot_id
                })

        browser.close()
        return bots

if __name__ == "__main__":
    bots = get_antinuke_bots()
    
    if bots:
        for bot in bots:
            print(f"🔹 {bot['name']} - {bot['bot_url']} (ID: {bot['bot_id']})")
    else:
        print("No bots found.")