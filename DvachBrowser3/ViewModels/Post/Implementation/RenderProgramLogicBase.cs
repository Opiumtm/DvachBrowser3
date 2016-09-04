using System;
using System.Collections.Generic;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовый класс для рендеринга программы.
    /// </summary>
    /// <typeparam name="T">Тип объекта для загрузки программы.</typeparam>
    /// <typeparam name="TElement">Тип элемента программы.</typeparam>
    /// <typeparam name="TAttr">Тип атрибута текста.</typeparam>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public abstract class RenderProgramLogicBase<T, TElement, TAttr, TResult>
        where TAttr : class
        where TElement : class 
    {
        /// <summary>
        /// Создать программу.
        /// </summary>
        /// <param name="nodes">Узлы программы.</param>
        /// <returns></returns>
        public TResult CreateProgram(IEnumerable<PostNodeBase> nodes)
        {
            var consumer = CreateConsumer();
            CreateProgram(consumer, nodes);
            return GetResult(consumer);
        }

        /// <summary>
        /// Создать программу.
        /// </summary>
        /// <param name="consumer">Объект для загрузки программы.</param>
        /// <param name="nodes">Узлы программы.</param>
        /// <returns></returns>
        public void CreateProgram(T consumer, IEnumerable<PostNodeBase> nodes)
        {
            bool lastBreak = false;
            RenderElements(consumer, nodes, ref lastBreak);
            Flush(consumer);
        }

        private void RenderElements(T consumer, IEnumerable<PostNodeBase> nodes, ref bool lastBreak)
        {
            if (nodes == null)
            {
                return;
            }
            foreach (var node in nodes)
            {
                RenderElement(consumer, node, ref lastBreak);
            }
        }

        private void RenderElement(T consumer, PostNodeBase node, ref bool lastBreak)
        {
            if (node == null)
            {
                return;
            }
            if (node is PostNodeBreak)
            {
                PushElement(consumer, LineBreak());
                lastBreak = true;
            }
            else if (node is PostTextNode)
            {
                var tnode = (PostTextNode)node;
                PushElement(consumer, PrintText(tnode.Text ?? ""));
                lastBreak = false;
            }
            else if (node is PostNode)
            {
                RenderAttributedNodes(consumer, (PostNode)node, ref lastBreak);
            }
            else if (node is PostNodeBoardLink)
            {
                var lnode = (PostNodeBoardLink)node;
                if (lnode.BoardLink != null)
                {
                    var latr = Link(lnode.BoardLink);
                    PushElement(consumer, AddAttribute(latr));
                    PushElement(consumer, PrintText(GetBoardLinkName(lnode.BoardLink)));
                    PushElement(consumer, RemoveAttribute(latr));
                    lastBreak = false;
                }
            }
        }

        private void RenderAttributedNodes(T consumer, PostNode node, ref bool lastBreak)
        {
            if (node == null)
            {
                return;
            }
            var nodeAttr = node.Attribute as PostNodeAttribute;
            var nodeLink = node.Attribute as PostNodeLinkAttribute;
            var nodeBLink = node.Attribute as PostNodeBoardLinkAttribute;

            TAttr attribute = null;

            TAttr linkAttribute = null;

            bool isParagraph = false;

            if (nodeAttr != null)
            {
                if (nodeAttr.Attribute == PostNodeBasicAttribute.Paragraph)
                {
                    isParagraph = true;
                }
                else
                {
                    attribute = CreateAttribute(nodeAttr.Attribute);
                }
            }

            if (nodeLink?.LinkUri != null)
            {
                linkAttribute = Link(nodeLink.LinkUri);
            }

            if (nodeBLink?.BoardLink != null)
            {
                linkAttribute = Link(nodeBLink.BoardLink);
            }

            if (attribute != null)
            {
                PushElement(consumer, AddAttribute(attribute));
            }

            if (linkAttribute != null)
            {
                PushElement(consumer, AddAttribute(linkAttribute));
            }

            if (!lastBreak && isParagraph)
            {
                PushElement(consumer, LineBreak());
                lastBreak = true;
            }

            RenderElements(consumer, node.Children, ref lastBreak);

            if (attribute != null)
            {
                PushElement(consumer, RemoveAttribute(attribute));
            }

            if (linkAttribute != null)
            {
                PushElement(consumer, RemoveAttribute(linkAttribute));
            }

            if (!lastBreak && isParagraph)
            {
                PushElement(consumer, LineBreak());
            }
        }


        private string GetBoardLinkName(BoardLinkBase link)
        {
            return ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetBackLinkDisplayString(link);
        }

        /// <summary>
        /// Объект перевода строки.
        /// </summary>
        /// <returns>Элемент.</returns>
        protected abstract TElement LineBreak();

        /// <summary>
        /// Печатать текст.
        /// </summary>
        /// <param name="text">Текст.</param>
        /// <returns>Элемент.</returns>
        protected abstract TElement PrintText(string text);

        /// <summary>
        /// Добавить атрибут.
        /// </summary>
        /// <param name="attr">Атрибут.</param>
        /// <returns>Элемент.</returns>
        protected abstract TElement AddAttribute(TAttr attr);

        /// <summary>
        /// Удалить атрибут.
        /// </summary>
        /// <param name="attr">Атрибут.</param>
        /// <returns>Элемент.</returns>
        protected abstract TElement RemoveAttribute(TAttr attr);

        /// <summary>
        /// Ссылка.
        /// </summary>
        /// <param name="link">Объкт ссылки.</param>
        /// <returns>Атрибут.</returns>
        protected abstract TAttr Link(BoardLinkBase link);

        /// <summary>
        /// Ссылка.
        /// </summary>
        /// <param name="uri">URI ссылки.</param>
        /// <returns>Атрибут.</returns>
        protected abstract TAttr Link(string uri);

        /// <summary>
        /// Создать атрибут.
        /// </summary>
        /// <param name="attr">Атрибут в тексте.</param>
        /// <returns>Атрибут.</returns>
        protected abstract TAttr CreateAttribute(PostNodeBasicAttribute attr);


        /// <summary>
        /// Загрузить элемент.
        /// </summary>
        /// <param name="consumer">Объект для загрузки.</param>
        /// <param name="element">Элемент.</param>
        protected abstract void PushElement(T consumer, TElement element);

        /// <summary>
        /// Создать объект загрузки программы.
        /// </summary>
        /// <returns>Объект загрузки.</returns>
        protected abstract T CreateConsumer();

        /// <summary>
        /// Завершить формирование программы.
        /// </summary>
        /// <param name="consumer">Объект загрузки.</param>
        protected abstract void Flush(T consumer);

        /// <summary>
        /// Получить результат.
        /// </summary>
        /// <param name="consumer">Объект загрузки.</param>
        /// <returns>Результат.</returns>
        protected abstract TResult GetResult(T consumer);
    }
}