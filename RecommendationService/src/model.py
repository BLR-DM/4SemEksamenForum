import pandas as pd
from faker import Faker


fake = Faker('da_DK')

NUM_POSTS = 30

posts_ids = list(range(1, NUM_POSTS +1 ))
titles = [fake.sentence(nb_words=6) for _ in range(NUM_POSTS)]
descriptions = [fake.paragraph(nb_sentences=3) for _ in range(NUM_POSTS)]

df_posts = pd.DataFrame({
    'post_id': posts_ids,
    'title': titles,
    'description': descriptions
})

csv_filename = 'post.csv'
df_posts.to_csv(csv_filename, index=False, sep=";")
print(f"Filen: {csv_filename} er blevet oprettet")

df_loaded_posts = pd.read_csv(csv_filename)
print("Indlæser data")
print("test123")

print(df_loaded_posts)