import React, { useState, useEffect } from 'react';
import styles from './EditCategory.module.css';
import categoryService from '../../../services/categoryService';
import { AiOutlineCloseCircle, AiOutlineEdit  } from 'react-icons/ai';
import { useAuth } from '../../../context/AuthContext';

interface Category {
  id: number;
  name: string;
  keywords: string[];
}

const EditCategories: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [clickedCategories, setClickedCategories] = useState<number[]>([]);
  const [clickedEditCategories, setClickedEditCategories] = useState<number[]>([]);
  const { user } = useAuth();


  useEffect(() => {
    const fetchCategories = async () => {
      try {
        console.log(user.userID);
        const data = await categoryService.getUserCategories(user.userID);
        const parsedCategories = JSON.parse(data.mappedCategoriesJson);
        const categoriesArray: Category[] = parsedCategories.map((category: { CategoryName: string; Keywords: string[]; }) => ({
          name: category.CategoryName,
          keywords: category.Keywords,
        }));
        setCategories(categoriesArray);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };
    fetchCategories();
  }, []);

  const handleDeleteCategory = (categoryId: number) => {
    setClickedCategories((prevClickedCategories) => {
      if (prevClickedCategories.includes(categoryId)) {
        return prevClickedCategories.filter((id) => id !== categoryId);
      } else {
        return [...prevClickedCategories, categoryId];
      }
    });
  };

  const handleDeleteKeyword = (categoryId: number, keyword: string) => {
    // Handle keyword deletion
  };

  const handleDeleteName = (categoryId: number, name : string) => {
    //handle
  }

  const handleEditCategory = (categoryId: number) => {
    setClickedEditCategories((prevClickedEditCategories) => {
      if (prevClickedEditCategories.includes(categoryId)) {
        return prevClickedEditCategories.filter((id) => id !== categoryId);
      } else {
        return [...prevClickedEditCategories, categoryId];
      }
    });
  };

  const handleEditKeyWords = (categoryId: number, keyword: string) => {
    // handle keywords logic
  }

  return (
    <div className={`${styles.card}`}>
      <h2 className={`text-3xl font-bold mb-4`}>Edit Categories</h2>
      <ul className={`${styles.categoryList}`}>
        {categories.map((category) => (
          <li key={category.name} className={`${styles.categoryItem}`}>
              <div className={`${styles.categoryHeader}`}>
                <strong className={`${styles.categoryName}`}>{category.name}</strong>
                {clickedEditCategories.includes(category.id) && (
                <AiOutlineEdit
                  className={`${styles.editIcon}`}
                  onClick={() => handleEditCategory(category.id)}
                />
              )}
               {clickedCategories.includes(category.id) && (
                <AiOutlineCloseCircle
                  className={`${styles.deleteKeywordButton}`}
                  onClick={() => handleDeleteName(category.id, category.name)}
                  />
              )}
                <div className={`${styles.keywordInput}`}>              
                {category.keywords.map((keyword, index) => (
                  <span key={index} className={`${styles.keywordTag}`}>
                    {clickedCategories.includes(category.id) && (
                      <AiOutlineCloseCircle
                        className={`${styles.deleteKeywordButton}`}
                        onClick={() => handleDeleteKeyword(category.id, keyword)}
                      />
                      )}
                       {clickedEditCategories.includes(category.id) && (
                      <AiOutlineEdit
                        className={`${styles.editIcon}`}
                        onClick={() => handleEditKeyWords(category.id, keyword)}
                      />
                    )}
                      {keyword}
                  </span>
                ))}
              </div>
                <div className={`${styles.buttons}`}>
                  <button
                    onClick={() => handleDeleteCategory(category.id)}
                    className={`${styles.deleteButton}`}
                  >
                    Delete
                  </button>
                  <button
                    onClick={() => handleEditCategory(category.id)}
                    className={`${styles.editButton}`}
                  >
                    Edit
                  </button>
                </div>                
              </div>
              
          </li>
        ))}
      </ul>
    </div>
  );
};

export default EditCategories;
