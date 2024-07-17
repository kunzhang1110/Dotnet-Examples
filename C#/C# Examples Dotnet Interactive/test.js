const saveArticleHandler = () => {
  const article = {
    title: title.text,
    body: body.text,
    viewed: 0,
    tags: JSON.stringify(tags),
  };

  const formData = new FormData();
  images.forEach((img, index) => {
    formData.append(`files[${index}]`, img.file);
  });
  Object.keys(article).forEach((key) => {
    formData.append(key, article[key]);
  });
  fetch(` /api/articles/${id}`, {
    method: "PUT",
    body: formData,
    headers,
    cache: "default",
  }).then(() => navigate(`/articles/${id}`));
};
